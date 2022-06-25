using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace SimpleString.Extenisons.Internal
{
    /// <summary>
    /// XML解析
    /// </summary>
    internal static class XMLResolverExtension
    {
        private const string ROOT = "/doc/members";
        private const string SUMMARY = "summary";
        private const string TYPE_PREFIX = "T:";
        private const string ATTRIBUTE = "name";

        /// <summary>
        /// XML解析
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static void XMLDocResolver(this Config config)
        {
            if (HandleType.XML != config.HandleType)
            {
                return;
            }

            if (!config.XMLDocPath?.Any() ?? true)
            {
                config.ErrorMsg = $"Param {nameof(config.XMLDocPath)} null or empty.";

                throw new ArgumentException("Param null or empty.", nameof(config.XMLDocPath));
            }

            var XMLDocs = new Dictionary<string, Dictionary<string, string>>();

            XmlDocument xmlDocument = new XmlDocument();
            foreach (var path in config.XMLDocPath)
            {
                if (!File.Exists(path))
                {
                    config.ErrorMsg = $@"""{path}"" not found.";

                    return;
                }

                xmlDocument.Load(path);

                // 成员根节点
                var root = xmlDocument.DocumentElement.SelectSingleNode(ROOT);
                if (0 == root.ChildNodes.Count)
                {
                    continue;
                }

                var nodes = root.ChildNodes.Cast<XmlNode>();

                // 获取所有类型节点
                var tNodes = nodes.Where(c => c.Attributes[ATTRIBUTE].Value.StartsWith(TYPE_PREFIX));
                foreach (var node in tNodes)
                {
                    var typeName = node.Attributes[ATTRIBUTE].Value.TrimStart(TYPE_PREFIX.ToArray());

                    // 当前类型节点对应成员
                    var childrens = nodes.Where(c => Regex.IsMatch(c.Attributes[ATTRIBUTE].Value, $@"^[^T]+?\:{typeName.Replace(".", @"\.")}\.[^\.]+?$"));
                    XMLDocs.Add(typeName, childrens.ToDictionary(c => Regex.Replace(c.Attributes[ATTRIBUTE].Value, @"^.+?\:", string.Empty), c => c.SelectSingleNode(SUMMARY).InnerText.Trim()));
                }
            }

            config.XMLDocContainer = XMLDocs;
            config.IsChecked = true;
        }
    }
}