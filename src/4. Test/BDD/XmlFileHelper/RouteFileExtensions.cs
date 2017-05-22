namespace PH.Well.BDD.XmlFileHelper
{
    using System;
    using System.IO;
    using System.Xml;

    public static class RouteFileExtensions
    {

        public static void AddElementsToRouteFile(string sourceFile, string parentElement, int nodeListPosition, string elementToAdd, string elementValue,
            string resultFile)
        {
            var routeFile = new XmlDocument();
            routeFile.Load(sourceFile);

            var routeHeaderNodeList = routeFile.GetElementsByTagName(parentElement);
            var routeHeaderCurrentParent = routeHeaderNodeList[nodeListPosition];

            var newNode = routeFile.CreateNode(XmlNodeType.Element, null, elementToAdd, null);
            newNode.InnerText = elementValue;
            routeHeaderCurrentParent.AppendChild(newNode);

            CreateResultDirectoryFromFileName(resultFile);

            routeFile.Save(resultFile);

        }


        public static void RemoveElementsFromRouteFile(string sourceFile, string parentElement, int nodeListPosition, string elementToRemove, 
                    string resultFile, bool isChildCollectionNode)
        {

            var routeFile = new XmlDocument();
            routeFile.Load(sourceFile);

            var routeHeaderNodeList = routeFile.GetElementsByTagName(parentElement);
            var routeHeaderCurrentParent = routeHeaderNodeList[nodeListPosition];

            if (isChildCollectionNode)
            {
                foreach (XmlNode collectionCurentChild in routeHeaderCurrentParent.ChildNodes)
                {
                    if (collectionCurentChild.Name == elementToRemove)
                        routeHeaderCurrentParent.RemoveChild(collectionCurentChild);
                }
            }
            else
            {
                foreach (XmlNode routeHeaderCuurentChild in routeHeaderCurrentParent.ChildNodes)
                {
                    if (routeHeaderCuurentChild.Name == elementToRemove)
                        routeHeaderCurrentParent.RemoveChild(routeHeaderCuurentChild);
                }
            }

            CreateResultDirectoryFromFileName(resultFile);

            routeFile.Save(resultFile);
        }

        public static void AlterElementInRouteFile(string sourceFile, string parentElement, int nodeListPosition, string elementToAlter, string newElementValue, string resultFile)
        {
            var routeFile = new XmlDocument();
            routeFile.Load(sourceFile);

            var routeHeaderNodeList = routeFile.GetElementsByTagName(parentElement);
            var routeHeaderCurrentParent = routeHeaderNodeList[nodeListPosition];

            foreach (XmlNode routeHeaderCuurentChild in routeHeaderCurrentParent.ChildNodes)
            {
                if (routeHeaderCuurentChild.Name == elementToAlter)
                    routeHeaderCuurentChild.InnerText = newElementValue;
            }

            CreateResultDirectoryFromFileName(resultFile);

            routeFile.Save(resultFile);
        }

        public static void DeleteTestRouteFiles(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            Array.ForEach(Directory.GetFiles(directory), File.Delete);
        }

        private static void CreateResultDirectoryFromFileName(string filename)
        {
            var resultDirectory = Path.GetDirectoryName(filename);
            if (!Directory.Exists(resultDirectory))
            {
                Directory.CreateDirectory(resultDirectory);
            }
        }

    }
}
