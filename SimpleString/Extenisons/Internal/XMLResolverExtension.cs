using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            if (HandleOptions.XML != config.HandleOptions)
            {
                return;
            }

            if (0 == config.XMLDocPath.Count)
            {
                config.ErrorMsg = $"Param {nameof(config.XMLDocPath)} is empty.";

                throw new ArgumentException("Param is empty.", nameof(config.XMLDocPath));
            }

            var XMLDocs = new Dictionary<string, Dictionary<string, string>>();
            var xmlDocument = new XmlDocument();

            try
            {
                foreach (var path in config.XMLDocPath)
                {
                    if (!File.Exists(path))
                    {
                        config.ErrorMsg = $"'{path}' not found.";

                        return;
                    }

                    xmlDocument.Load(path);

                    // 成员根节点
                    var root = xmlDocument.DocumentElement.SelectSingleNode(ROOT);
                    if (0 == root.ChildNodes.Count)
                    {
                        continue;
                    }

                    var nodes = root.ChildNodes.Cast<XmlNode>().ToList();

                    // 获取所有类型节点
                    var tNodes = nodes.Where(c => c.Attributes[ATTRIBUTE].Value.StartsWith(TYPE_PREFIX));

                    foreach (var node in tNodes)
                    {
                        var typeName = Regex.Replace(node.Attributes[ATTRIBUTE].Value, $"^{TYPE_PREFIX}", string.Empty);

                        // 当前类型节点对应成员
                        var childrens = nodes.Where(c => Regex.IsMatch(c.Attributes[ATTRIBUTE].Value, $@"^[^TM]+?\:{typeName.Replace(".", @"\.")}\.[^\.]+?$"));
                        if (childrens.Any())
                        {
                            XMLDocs[typeName] = childrens.ToDictionary(c => Regex.Replace(c.Attributes[ATTRIBUTE].Value, @"^.+?\:", string.Empty), c => c.SelectSingleNode(SUMMARY).InnerText.Trim());
                        }
                    }
                }

                config.XMLDocContainer = XMLDocs;
                config.IsChecked = true;
            }
            catch (Exception ex)
            {
                config.ErrorMsg = $"Xml load error: {ex.Message}";
            }
        }
    }
}