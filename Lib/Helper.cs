﻿using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace ManiaTemplates.Lib;

public class Helper
{
    internal static string Hash(string input)
    {
        var hash = new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(input));
        return string.Concat(hash.Select(b => b.ToString("x2")))[..32];
    }

    public static string RandomString(int length = 16)
    {
        var random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    internal static bool UsesComponents(XmlNode node, ComponentList components)
    {
        foreach (XmlNode child in node.ChildNodes)
        {
            return UsesComponents(child, components);
        }

        return components.ContainsKey(node.Name);
    }

    public static string PrettyXml(string? uglyXml = null)
    {
        if (uglyXml == null || uglyXml.Trim().Length == 0)
        {
            return "";
        }

        var stringBuilder = new StringBuilder();
        var element = XElement.Parse(uglyXml);

        var settings = new XmlWriterSettings();
        settings.OmitXmlDeclaration = true;
        settings.Indent = true;
        settings.NewLineOnAttributes = false;

        using (var xmlWriter = XmlWriter.Create(stringBuilder, settings))
        {
            element.Save(xmlWriter);
        }

        return stringBuilder.ToString();
    }

    public static string EscapeXml(string xml)
    {
        var stringBuilder = new StringBuilder();
        var element = XElement.Parse(xml);

        var settings = new XmlWriterSettings();
        settings.OmitXmlDeclaration = true;
        settings.Indent = true;
        settings.NewLineOnAttributes = false;
        settings.ConformanceLevel = ConformanceLevel.Fragment;
        settings.Encoding = Encoding.Unicode;

        using (var xmlWriter = XmlWriter.Create(stringBuilder, settings))
        {
            element.Save(xmlWriter);
        }

        return stringBuilder.ToString();
    }
}