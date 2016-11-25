CREATE PROC [dbo].[AuditTrigger] 
  @SourceTableName varchar(255),
  @Deletes bit,
  @Inserts bit,
  @Updates bit  
WITH RECOMPILE
AS
BEGIN
                SET NOCOUNT ON
                
                DECLARE @sql NVARCHAR(MAX),
                                                @tableName NVARCHAR(MAX) = @SourceTableName,
                                                @auditTableName NVARCHAR(MAX) = @SourceTableName + '_Audit',
                                                @auditTriggerName NVARCHAR(MAX) = 'Audit_' + @SourceTableName,
                                                @auditTriggerFor NVARCHAR(MAX) = '',
                                                @sourceTableExists BIT,
                                                @auditTableExists BIT,
                                                @auditTriggerNameExists BIT,
                                                @columnNames NVARCHAR(MAX),
                                                @name NVARCHAR(MAX),
                                                @originalColumns NVARCHAR(MAX) = '',
                                                @timestampColumn NVARCHAR(MAX), 
                                                @uniqueIdentityColumn NVARCHAR(MAX),
                                                @varbinaryColumn NVARCHAR(MAX)
                
                DECLARE @columns TABLE(name NVARCHAR(255))
                DECLARE @alterStatements TABLE(sql NVARCHAR(4000))
                DECLARE @newColumns TABLE(sql NVARCHAR(4000))
                
                SET @sourceTableExists = (SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @SourceTableName)
                SET @auditTableExists = (SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @auditTableName)
                SET @auditTriggerNameExists = (SELECT COUNT(*) FROM sys.triggers WHERE name IN (@auditTriggerName))    

                IF(@sourceTableExists = 0)
                BEGIN
                                RAISERROR
                                                (N'Source table not found',
                                                10, -- Severity.
                                                1 -- State.
                                                );
                                                RETURN               
                END

                -- Remove triggers, they get re-created later
                IF(@auditTriggerNameExists = 1)
                BEGIN
                                SET @sql = 'IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(''' + @auditTriggerName + ''') AND type = ''TR'') BEGIN DROP TRIGGER ' + @auditTriggerName + ' END'
                                EXEC sp_executesql @sql
                END       

                -- Create the audit table if it does not exist
                IF(@auditTableExists = 0)
                BEGIN
                                -- create the new table as a select * from the original
                                SET @sql = 'SELECT REPLICATE('' '',500) AS transactionid, getdate() AS date, REPLICATE('' '',20) AS operation, * INTO ' + @auditTableName + ' FROM [' + @tableName + '] WHERE 1=2'
                                --print @sql
                                EXEC sp_executesql @sql
                END
                ELSE
                BEGIN
                                -- Audit table exists so add any new columns to it                             
                                INSERT INTO @newColumns (sql)
                                SELECT 
                                                'ALTER TABLE ' + @auditTableName + ' ADD ' + COLUMN_NAME + ' ' +
                                                CASE 
                                                  WHEN DATA_TYPE = 'datetime' THEN 'datetime'
                                                  WHEN DATA_TYPE = 'nvarchar' THEN 'nvarchar(' + REPLACE(CAST(CHARACTER_MAXIMUM_LENGTH as NVARCHAR) +')', '-1','MAX')  
                                                  WHEN DATA_TYPE = 'varchar' THEN 'nvarchar(' + REPLACE(CAST(CHARACTER_MAXIMUM_LENGTH as NVARCHAR) +')', '-1','MAX') 
                                                  ELSE DATA_TYPE 
                                                END  
                                                + ' NULL'
                                                FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME IN (
                                                                SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @tableName
                                                                EXCEPT
                                                                SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @auditTableName AND COLUMN_NAME NOT IN ('date','operation','transactionid')
                                                )
                                                AND TABLE_NAME = @tableName
                                
                                WHILE EXISTS (SELECT * FROM @newColumns)
                                BEGIN
                                                SELECT TOP 1 @sql = sql FROM @newColumns

                                                EXEC sp_executesql @sql

                                                DELETE @newColumns WHERE sql = @sql
                                END
                END

                -- if it has a timestamp column remove it and add it back in as a binary(8)    
                SELECT @timestampColumn = COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS 
                WHERE TABLE_NAME = @auditTableName 
                AND data_type = 'timestamp'                    

                IF(@timestampColumn IS NOT NULL)
                BEGIN   
                                SET @sql = 'ALTER TABLE ' + @auditTableName + ' DROP COLUMN ' + @timestampColumn
                                EXEC sp_executesql @sql
                                
                                SET @sql = 'ALTER TABLE ' + @auditTableName + ' ADD ' + @timestampColumn + ' binary(8)'
                                EXEC sp_executesql @sql
                END       
                                
                -- Turn off uniqueidentity columns                          
                SELECT @uniqueIdentityColumn = COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS 
                WHERE TABLE_NAME = @auditTableName 
                AND COLUMNPROPERTY(object_id(TABLE_NAME), COLUMN_NAME, 'IsIdentity') = 1    

                IF(@uniqueIdentityColumn IS NOT NULL)
                BEGIN   
                                SET @sql = 'ALTER TABLE ' + @auditTableName + ' DROP COLUMN ' + @uniqueIdentityColumn
                                EXEC sp_executesql @sql
                                
                                SET @sql = 'ALTER TABLE ' + @auditTableName + ' ADD ' + @uniqueIdentityColumn + ' int'
                                EXEC sp_executesql @sql
                END       
                
                SELECT @varbinaryColumn = COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS 
                WHERE TABLE_NAME = @auditTableName 
                AND DATA_TYPE = 'varbinary'    

                IF(@varbinaryColumn IS NOT NULL)
                BEGIN   
                                SET @sql = 'ALTER TABLE ' + @auditTableName + ' DROP COLUMN ' + @varbinaryColumn
                                EXEC sp_executesql @sql                            

                                SET @sql = 'ALTER TABLE ' + @auditTableName + ' ADD ' + @varbinaryColumn + ' NVARCHAR(1)'
                                EXEC sp_executesql @sql                            
                END       

                -- Remove all NOT NULL constraints
                INSERT INTO @alterStatements (sql)
                                SELECT 
                                'ALTER TABLE ' + @auditTableName + ' ALTER COLUMN [' + column_name + '] ' +
                                CASE 
                                  WHEN data_type = 'datetime' THEN 'datetime'
                                  WHEN data_type = 'nvarchar' THEN 'nvarchar(' + REPLACE(CAST(character_maximum_length as NVARCHAR) +')', '-1','MAX')  
                                  WHEN data_type = 'varchar' THEN 'nvarchar(' + REPLACE(CAST(character_maximum_length as NVARCHAR) +')', '-1','MAX') 
                                  ELSE data_type 
                                END  
                                + ' NULL'
                                FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @auditTableName AND IS_NULLABLE = 'NO' ORDER BY ORDINAL_POSITION 

                WHILE EXISTS (SELECT * FROM @alterStatements)
                BEGIN
                                SELECT TOP 1 @sql = sql FROM @alterStatements

                                EXEC sp_executesql @sql

                                DELETE @alterStatements WHERE sql = @sql
                END
                
                SET @originalColumns = ''
                
                INSERT INTO @columns (name)
                                SELECT '[' + COLUMN_NAME + ']' FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @tableName ORDER BY ordinal_position
                                --SELECT '[' + COLUMN_NAME + ']' FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @tableName AND DATA_TYPE <> 'varbinary' ORDER BY ordinal_position

                WHILE EXISTS (SELECT * FROM @columns)
                BEGIN
                                SELECT TOP 1 @name = name FROM @columns
                                
                                SET @originalColumns = @originalColumns + ', ' + @name                            

                                DELETE @columns WHERE name = @name
                END                       
                                
                SET @columnNames = replace(replace(@originalColumns,' ',''),',',' ,i.')
                
                --print @originalColumns
                --print @columnNames
                                
                IF(@Inserts = 1) SET @auditTriggerFor = @auditTriggerFor + ' INSERT'

                IF(@Updates = 1) SET @auditTriggerFor = @auditTriggerFor + ', UPDATE'
                                
                IF(@Deletes = 1) SET @auditTriggerFor = @auditTriggerFor + ', DELETE'
                
                --print @auditTriggerFor
                
                SET @sql =        'CREATE TRIGGER [dbo].[' + @auditTriggerName + '] on [dbo].[' + @tableName + '] FOR ' + @auditTriggerFor + ' AS ' + char(13) + char(10)
                SET @sql = @sql + 'BEGIN ' + char(13) + char(10)
                
                SET @sql = @sql + 'IF (@@ROWCOUNT  = 0)  RETURN ' + char(13) + char(10)
                
                SET @sql = @sql + 'DECLARE @tranId NVARCHAR(20), ' + char(13) + char(10)
                SET @sql = @sql + '        @operation NVARCHAR(20), ' + char(13) + char(10)
                SET @sql = @sql + '        @subOperation NVARCHAR(20), ' + char(13) + char(10)
                SET @sql = @sql + '        @name NVARCHAR(4000) ' + char(13) + char(10)
                SET @sql = @sql + 'DECLARE @sqlStatements TABLE(name NVARCHAR(4000)) ' + char(13) + char(10)
                
                SET @sql = @sql + 'SELECT @tranId = transaction_id FROM sys.dm_tran_current_transaction ' + char(13) + char(10)
                
                SET @sql = @sql + 'SELECT * INTO #ins FROM inserted ' + char(13) + char(10)
                SET @sql = @sql + 'SELECT * INTO #del FROM deleted ' + char(13) + char(10)

                SET @sql = @sql + 'IF EXISTS (SELECT * FROM inserted) ' + char(13) + char(10)
                SET @sql = @sql + 'BEGIN ' + char(13) + char(10)
                SET @sql = @sql + '          IF EXISTS (SELECT * FROM deleted) ' + char(13) + char(10)
                SET @sql = @sql + '          BEGIN ' + char(13) + char(10)
                SET @sql = @sql + '                          SELECT @operation = ''BEFOREUPDATE'' ' + char(13) + char(10)
                SET @sql = @sql + '                          SELECT @subOperation = ''AFTERUPDATE'' ' + char(13) + char(10)
                SET @sql = @sql + '          END ' + char(13) + char(10)
                SET @sql = @sql + '          ELSE ' + char(13) + char(10)
                SET @sql = @sql + '          BEGIN ' + char(13) + char(10)
                SET @sql = @sql + '                          SELECT @operation = ''INSERT'' ' + char(13) + char(10)
                SET @sql = @sql + '          END ' + char(13) + char(10)
                SET @sql = @sql + 'END ' + char(13) + char(10)
                SET @sql = @sql + 'ELSE IF EXISTS (SELECT * FROM deleted) ' + char(13) + char(10)
                SET @sql = @sql + 'BEGIN ' + char(13) + char(10)
                SET @sql = @sql + '          SELECT @operation = ''DELETE'' ' + char(13) + char(10)
                SET @sql = @sql + 'END ' + char(13) + char(10)
                                
                SET @sql = @sql + 'IF(@operation = ''DELETE'' OR @operation = ''BEFOREUPDATE'') INSERT INTO @sqlStatements (name) SELECT ''INSERT INTO ' + @auditTableName + ' (transactionid, date, operation' + @originalColumns + ') SELECT '' + @tranid + '', getdate(), '''''' + @operation + '''''', i.* FROM #del i '' ' + char(13) + char(10)
                SET @sql = @sql + 'IF(@operation = ''INSERT'' OR @operation = ''BEFOREUPDATE'') INSERT INTO @sqlStatements (name) SELECT ''INSERT INTO ' + @auditTableName + ' (transactionid, date, operation' + @originalColumns + ') SELECT '' + @tranid + '', getdate(), '''''' + IsNull(@suboperation, @operation) + '''''', i.* FROM #ins i '' ' + char(13) + char(10)  
                                
                SET @sql = @sql + 'SET ANSI_WARNINGS OFF ' + char(13) + char(10)

                SET @sql = @sql + 'WHILE EXISTS (SELECT * FROM @sqlStatements) ' + char(13) + char(10)
                SET @sql = @sql + 'BEGIN ' + char(13) + char(10)
                SET @sql = @sql + '          SELECT TOP 1 @name = name FROM @sqlStatements ' + char(13) + char(10)
                SET @sql = @sql + '          EXEC sp_executesql @name ' + char(13) + char(10)
                SET @sql = @sql + '          DELETE @sqlStatements WHERE name = @name ' + char(13) + char(10)
                SET @sql = @sql + 'END ' + char(13) + char(10)
                
                SET @sql = @sql + 'SET ANSI_WARNINGS ON ' + char(13) + char(10)

                SET @sql = @sql + 'END ' + char(13) + char(10)
                
                EXEC sp_executesql @sql
END
