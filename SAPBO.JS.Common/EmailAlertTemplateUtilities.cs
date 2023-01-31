using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPBO.JS.Common
{
    public class EmailAlertTemplateModel0Data
    {
        public string Name { get; set; }

        public string Value { get; set; }
    }

    public class EmailAlertTemplateModel0Block
    {
        public EmailAlertTemplateModel0Data LeftBlock { get; set; }

        public EmailAlertTemplateModel0Data RightBlock { get; set; }
    }

    public class EmailAlertTemplateModel0Detail
    {
        public string DetailName { get; set; }

        public string DetailSecondName { get; set; }

        public string DetailSecondValue { get; set; }

        public string DetailText { get; set; }

        public string DetailRightName0 { get; set; }

        public string DetailRightValue0 { get; set; }

        public string DetailRightName1 { get; set; }

        public string DetailRightValue1 { get; set; }
    }

    public class EmailAlertTemplateModel0
    {
        public string HeadTitle { get; set; }

        public string HeadSecondLine { get; set; }

        public string AlertImageLink { get; set; }

        public string AlertTitle { get; set; }

        public string AlertText { get; set; }

        public List<EmailAlertTemplateModel0Data> Heads { get; set; }

        public string DetailTitle { get; set; }

        public List<EmailAlertTemplateModel0Detail> Details { get; set; }

        public List<EmailAlertTemplateModel0Data> Footers { get; set; }

        public string FooterTitle { get; set; }

        public string FooterText { get; set; }

        public List<EmailAlertTemplateModel0Block> FooterBlocks { get; set; }
    }

    public static class EmailAlertTemplateUtilities
    {
        public static string EmailAlertTemplateModel0Builder(EmailAlertTemplateModel0 emailAlertTemplateModel0)
        {
            var init = File.ReadAllText(Utilities.GetWebPath("EmailTemplates", "_EmailAlertTemplate_M0_Init.html"));
            var header = File.ReadAllText(Utilities.GetWebPath("EmailTemplates", "_EmailAlertTemplate_M0_Header.html"));
            var headerValues = File.ReadAllText(Utilities.GetWebPath("EmailTemplates", "_EmailAlertTemplate_M0_HeaderValues.html"));
            var spaceBlock = File.ReadAllText(Utilities.GetWebPath("EmailTemplates", "_EmailAlertTemplate_M0_SpaceBlock.html"));
            var line = File.ReadAllText(Utilities.GetWebPath("EmailTemplates", "_EmailAlertTemplate_M0_Line.html"));
            var detail = File.ReadAllText(Utilities.GetWebPath("EmailTemplates", "_EmailAlertTemplate_M0_Detail.html"));
            var product = File.ReadAllText(Utilities.GetWebPath("EmailTemplates", "_EmailAlertTemplate_M0_Product.html"));
            var footer = File.ReadAllText(Utilities.GetWebPath("EmailTemplates", "_EmailAlertTemplate_M0_Footer.html"));
            var footerValues = File.ReadAllText(Utilities.GetWebPath("EmailTemplates", "_EmailAlertTemplate_M0_FooterValues.html"));
            var twoBlock = File.ReadAllText(Utilities.GetWebPath("EmailTemplates", "_EmailAlertTemplate_M0_TwoBlock.html"));
            var singleBlock = File.ReadAllText(Utilities.GetWebPath("EmailTemplates", "_EmailAlertTemplate_M0_SingleBlock.html"));
            var contact = File.ReadAllText(Utilities.GetWebPath("EmailTemplates", "_EmailAlertTemplate_M0_Contact.html"));
            var copyright = File.ReadAllText(Utilities.GetWebPath("EmailTemplates", "_EmailAlertTemplate_M0_Copyright.html"));
            var end = File.ReadAllText(Utilities.GetWebPath("EmailTemplates", "_EmailAlertTemplate_M0_End.html"));

            var sb = new StringBuilder();
            sb.Append(init);

            sb.Append(header
                .Replace("[HeadTitle]", emailAlertTemplateModel0.HeadTitle)
                .Replace("[HeadSecondLine]", emailAlertTemplateModel0.HeadSecondLine)
                .Replace("[AlertImageLink]", emailAlertTemplateModel0.AlertImageLink)
                .Replace("[AlertTitle]", emailAlertTemplateModel0.AlertTitle)
                .Replace("[AlertText]", emailAlertTemplateModel0.AlertText)
            );

            foreach (var head in emailAlertTemplateModel0.Heads)
            {
                sb.Append(headerValues
                    .Replace("[HeadName]", head.Name)
                    .Replace("[HeadValue]", head.Value)
                );
            }

            sb.Append(spaceBlock);
            sb.Append(line);
            sb.Append(detail.Replace("[DetailTitle]", emailAlertTemplateModel0.DetailTitle));
            sb.Append(line);

            foreach (var item in emailAlertTemplateModel0.Details)
            {
                sb.Append(product
                    .Replace("[DetailName]", item.DetailName)
                    .Replace("[DetailSecondName]", item.DetailSecondName)
                    .Replace("[DetailSecondValue]", item.DetailSecondValue)
                    .Replace("[DetailText]", item.DetailText)
                    .Replace("[DetailRightName0]", item.DetailRightName0)
                    .Replace("[DetailRightValue0]", item.DetailRightValue0)
                    .Replace("[DetailRightName1]", item.DetailRightName1)
                    .Replace("[DetailRightValue1]", item.DetailRightValue1)
                );
                sb.Append(line);
            }

            foreach (var foot in emailAlertTemplateModel0.Footers)
            {
                sb.Append(footerValues
                    .Replace("[FooterName]", foot.Name)
                    .Replace("[FooterValue]", foot.Value)
                );
            }

            sb.Append(line);
            sb.Append(spaceBlock);

            sb.Append(footer
                .Replace("[FooterTitle]", emailAlertTemplateModel0.FooterTitle)
                .Replace("[FooterText]", emailAlertTemplateModel0.FooterText)
            );

            if (emailAlertTemplateModel0.FooterBlocks != null && emailAlertTemplateModel0.FooterBlocks.Any())
            {
                foreach (var blocks in emailAlertTemplateModel0.FooterBlocks)
                {
                    if (blocks.LeftBlock != null && blocks.RightBlock != null)
                    {
                        sb.Append(twoBlock
                            .Replace("[LeftBlockTitle]", blocks.LeftBlock.Name)
                            .Replace("[LeftBlockText]", blocks.LeftBlock.Value)
                            .Replace("[RightBlockTitle]", blocks.RightBlock.Name)
                            .Replace("[RightBlockText]", blocks.RightBlock.Value)
                        );
                    }
                    else
                    {
                        sb.Append(singleBlock
                            .Replace("[BlockTitle]", blocks.LeftBlock.Name)
                            .Replace("[BlockText]", blocks.LeftBlock.Value)
                        );
                    }
                }
            }

            sb.Append(contact);
            sb.Append(copyright);
            sb.Append(end);

            return sb.ToString();
        }
    }
}
