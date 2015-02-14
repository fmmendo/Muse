﻿using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace Krempel.WP7.Core.Controls
{
    [TemplatePart(Name = HtmlTextBlock.PART_ItemsControl, Type = typeof(ItemsControl))]
    public class HtmlTextBlock : Control
    {
        private ItemsControl internalItemsControl;
        private const string PART_ItemsControl = "PART_ItemsControl";

        static HtmlTextBlock()
        {
            HtmlProperty = DependencyProperty.Register("Html", typeof(string), typeof(HtmlTextBlock), new PropertyMetadata(HtmlChanged));
            NavigationCommandProperty = DependencyProperty.Register("NavigationCommand", typeof(ICommand), typeof(HtmlTextBlock), new PropertyMetadata(NavigationCommandChanged));

            HyperlinkFontFamilyProperty = DependencyProperty.Register("HyperlinkFontFamily", typeof(FontFamily), typeof(HtmlTextBlock), new PropertyMetadata(null));
            HyperlinkFontSizeProperty = DependencyProperty.Register("HyperlinkFontSize", typeof(double?), typeof(HtmlTextBlock), new PropertyMetadata(null));
            HyperlinkFontStretchProperty = DependencyProperty.Register("HyperlinkFontStretch", typeof(FontStretch), typeof(HtmlTextBlock), new PropertyMetadata(null));
            HyperlinkFontStyleProperty = DependencyProperty.Register("HyperlinkFontStyle", typeof(FontStyle), typeof(HtmlTextBlock), new PropertyMetadata(null));
            HyperlinkFontWeightProperty = DependencyProperty.Register("HyperlinkFontWeight", typeof(FontWeight), typeof(HtmlTextBlock), new PropertyMetadata(null));
            HyperlinkForegroundProperty = DependencyProperty.Register("HyperlinkForeground", typeof(Brush), typeof(HtmlTextBlock), new PropertyMetadata(null));

            H1FontFamilyProperty = DependencyProperty.Register("H1FontFamily", typeof(FontFamily), typeof(HtmlTextBlock), new PropertyMetadata(null));
            H1FontSizeProperty = DependencyProperty.Register("H1FontSize", typeof(double?), typeof(HtmlTextBlock), new PropertyMetadata(null));
            H1FontStretchProperty = DependencyProperty.Register("H1FontStretch", typeof(FontStretch), typeof(HtmlTextBlock), new PropertyMetadata(null));
            H1FontStyleProperty = DependencyProperty.Register("H1FontStyle", typeof(FontStyle), typeof(HtmlTextBlock), new PropertyMetadata(null));
            H1FontWeightProperty = DependencyProperty.Register("H1FontWeight", typeof(FontWeight), typeof(HtmlTextBlock), new PropertyMetadata(null));
            H1ForegroundProperty = DependencyProperty.Register("H1Foreground", typeof(Brush), typeof(HtmlTextBlock), new PropertyMetadata(null));

            H2FontFamilyProperty = DependencyProperty.Register("H2FontFamily", typeof(FontFamily), typeof(HtmlTextBlock), new PropertyMetadata(null));
            H2FontSizeProperty = DependencyProperty.Register("H2FontSize", typeof(double?), typeof(HtmlTextBlock), new PropertyMetadata(null));
            H2FontStretchProperty = DependencyProperty.Register("H2FontStretch", typeof(FontStretch), typeof(HtmlTextBlock), new PropertyMetadata(null));
            H2FontStyleProperty = DependencyProperty.Register("H2FontStyle", typeof(FontStyle), typeof(HtmlTextBlock), new PropertyMetadata(null));
            H2FontWeightProperty = DependencyProperty.Register("H2FontWeight", typeof(FontWeight), typeof(HtmlTextBlock), new PropertyMetadata(null));
            H2ForegroundProperty = DependencyProperty.Register("H2Foreground", typeof(Brush), typeof(HtmlTextBlock), new PropertyMetadata(null));

            H3FontFamilyProperty = DependencyProperty.Register("H3FontFamily", typeof(FontFamily), typeof(HtmlTextBlock), new PropertyMetadata(null));
            H3FontSizeProperty = DependencyProperty.Register("H3FontSize", typeof(double?), typeof(HtmlTextBlock), new PropertyMetadata(null));
            H3FontStretchProperty = DependencyProperty.Register("H3FontStretch", typeof(FontStretch), typeof(HtmlTextBlock), new PropertyMetadata(null));
            H3FontStyleProperty = DependencyProperty.Register("H3FontStyle", typeof(FontStyle), typeof(HtmlTextBlock), new PropertyMetadata(null));
            H3FontWeightProperty = DependencyProperty.Register("H3FontWeight", typeof(FontWeight), typeof(HtmlTextBlock), new PropertyMetadata(null));
            H3ForegroundProperty = DependencyProperty.Register("H3Foreground", typeof(Brush), typeof(HtmlTextBlock), new PropertyMetadata(null));

            BlockQuoteBackgroundProperty = DependencyProperty.Register("BlockQuoteBackground", typeof(Brush), typeof(HtmlTextBlock), new PropertyMetadata(null));
        }

        public HtmlTextBlock()
        {
            DefaultStyleKey = typeof(HtmlTextBlock);
            navigationHandler = new RoutedEventHandler(OnNavigationRequested);
        }

        #region HtmlProperty

        public static readonly DependencyProperty HtmlProperty;

        public string Html
        {
            get { return (string)GetValue(HtmlProperty); }
            set { SetValue(HtmlProperty, value); }
        }

        private static void HtmlChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            HtmlTextBlock instance = (HtmlTextBlock)o;
            if (instance.internalItemsControl != null)
                instance.AppendHtml(e.NewValue.ToString());
        }

        #endregion

        #region NavigationCommandProperty

        public static readonly DependencyProperty NavigationCommandProperty;

        public ICommand NavigationCommand
        {
            get { return (ICommand)GetValue(NavigationCommandProperty); }
            set { SetValue(NavigationCommandProperty, value); }
        }

        private static void NavigationCommandChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            HtmlTextBlock instance = (HtmlTextBlock)o;

            if (instance.textBoxes != null)
            {
                foreach (var textBlock in instance.textBoxes)
                {
                    foreach (var hyperlink in textBlock.GetChildInlines().OfType<Hyperlink>())
                    {
                        hyperlink.Command = instance.NavigationCommand;
                    }
                }
            }
        }

        #endregion

        #region NavigationEvent

        private event EventHandler<NavigationEventArgs> navigationEvent;

        public event EventHandler<NavigationEventArgs> NavigationRequested
        {
            add { navigationEvent += value; }
            remove { navigationEvent -= value; }
        }

        private RoutedEventHandler navigationHandler = null;

        public void OnNavigationRequested(object sender, RoutedEventArgs e)
        {
            if (navigationEvent != null)
            {
                Hyperlink link = e.OriginalSource as Hyperlink;

                if (link != null && link.CommandParameter != null)
                {
                    navigationEvent(this, new NavigationEventArgs(link, link.CommandParameter as Uri));
                }
            }
        }

        #endregion

        #region HyperlinkFontProperties

        public static readonly DependencyProperty HyperlinkFontFamilyProperty;

        public FontFamily HyperlinkFontFamily
        {
            get { return (FontFamily)GetValue(HyperlinkFontFamilyProperty); }
            set { SetValue(HyperlinkFontFamilyProperty, value); }
        }

        public static readonly DependencyProperty HyperlinkFontSizeProperty;

        public double? HyperlinkFontSize
        {
            get { return (double?)GetValue(HyperlinkFontSizeProperty); }
            set { SetValue(HyperlinkFontSizeProperty, value); }
        }

        public static readonly DependencyProperty HyperlinkFontStretchProperty;

        public FontStretch HyperlinkFontStretch
        {
            get { return (FontStretch)GetValue(HyperlinkFontStretchProperty); }
            set { SetValue(HyperlinkFontStretchProperty, value); }
        }

        public static readonly DependencyProperty HyperlinkFontStyleProperty;

        public FontStyle HyperlinkFontStyle
        {
            get { return (FontStyle)GetValue(HyperlinkFontStyleProperty); }
            set { SetValue(HyperlinkFontStyleProperty, value); }
        }

        public static readonly DependencyProperty HyperlinkFontWeightProperty;

        public FontWeight HyperlinkFontWeight
        {
            get { return (FontWeight)GetValue(HyperlinkFontWeightProperty); }
            set { SetValue(HyperlinkFontWeightProperty, value); }
        }

        public static readonly DependencyProperty HyperlinkForegroundProperty;

        public Brush HyperlinkForeground
        {
            get { return (Brush)GetValue(HyperlinkForegroundProperty); }
            set { SetValue(HyperlinkForegroundProperty, value); }
        }

        #endregion

        #region H1FontProperties

        public static readonly DependencyProperty H1FontFamilyProperty;

        public FontFamily H1FontFamily
        {
            get { return (FontFamily)GetValue(H1FontFamilyProperty); }
            set { SetValue(H1FontFamilyProperty, value); }
        }

        public static readonly DependencyProperty H1FontSizeProperty;

        public double? H1FontSize
        {
            get { return (double?)GetValue(H1FontSizeProperty); }
            set { SetValue(H1FontSizeProperty, value); }
        }

        public static readonly DependencyProperty H1FontStretchProperty;

        public FontStretch H1FontStretch
        {
            get { return (FontStretch)GetValue(H1FontStretchProperty); }
            set { SetValue(H1FontStretchProperty, value); }
        }

        public static readonly DependencyProperty H1FontStyleProperty;

        public FontStyle H1FontStyle
        {
            get { return (FontStyle)GetValue(H1FontStyleProperty); }
            set { SetValue(H1FontStyleProperty, value); }
        }

        public static readonly DependencyProperty H1FontWeightProperty;

        public FontWeight H1FontWeight
        {
            get { return (FontWeight)GetValue(H1FontWeightProperty); }
            set { SetValue(H1FontWeightProperty, value); }
        }

        public static readonly DependencyProperty H1ForegroundProperty;

        public Brush H1Foreground
        {
            get { return (Brush)GetValue(H1ForegroundProperty); }
            set { SetValue(H1ForegroundProperty, value); }
        }

        #endregion

        #region H2FontProperties

        public static readonly DependencyProperty H2FontFamilyProperty;

        public FontFamily H2FontFamily
        {
            get { return (FontFamily)GetValue(H2FontFamilyProperty); }
            set { SetValue(H2FontFamilyProperty, value); }
        }

        public static readonly DependencyProperty H2FontSizeProperty;

        public double? H2FontSize
        {
            get { return (double?)GetValue(H2FontSizeProperty); }
            set { SetValue(H2FontSizeProperty, value); }
        }

        public static readonly DependencyProperty H2FontStretchProperty;

        public FontStretch H2FontStretch
        {
            get { return (FontStretch)GetValue(H2FontStretchProperty); }
            set { SetValue(H2FontStretchProperty, value); }
        }

        public static readonly DependencyProperty H2FontStyleProperty;

        public FontStyle H2FontStyle
        {
            get { return (FontStyle)GetValue(H2FontStyleProperty); }
            set { SetValue(H2FontStyleProperty, value); }
        }

        public static readonly DependencyProperty H2FontWeightProperty;

        public FontWeight H2FontWeight
        {
            get { return (FontWeight)GetValue(H2FontWeightProperty); }
            set { SetValue(H2FontWeightProperty, value); }
        }

        public static readonly DependencyProperty H2ForegroundProperty;

        public Brush H2Foreground
        {
            get { return (Brush)GetValue(H2ForegroundProperty); }
            set { SetValue(H2ForegroundProperty, value); }
        }

        #endregion

        #region H3FontProperties

        public static readonly DependencyProperty H3FontFamilyProperty;

        public FontFamily H3FontFamily
        {
            get { return (FontFamily)GetValue(H3FontFamilyProperty); }
            set { SetValue(H3FontFamilyProperty, value); }
        }

        public static readonly DependencyProperty H3FontSizeProperty;

        public double? H3FontSize
        {
            get { return (double?)GetValue(H3FontSizeProperty); }
            set { SetValue(H3FontSizeProperty, value); }
        }

        public static readonly DependencyProperty H3FontStretchProperty;

        public FontStretch H3FontStretch
        {
            get { return (FontStretch)GetValue(H3FontStretchProperty); }
            set { SetValue(H3FontStretchProperty, value); }
        }

        public static readonly DependencyProperty H3FontStyleProperty;

        public FontStyle H3FontStyle
        {
            get { return (FontStyle)GetValue(H3FontStyleProperty); }
            set { SetValue(H3FontStyleProperty, value); }
        }

        public static readonly DependencyProperty H3FontWeightProperty;

        public FontWeight H3FontWeight
        {
            get { return (FontWeight)GetValue(H3FontWeightProperty); }
            set { SetValue(H3FontWeightProperty, value); }
        }

        public static readonly DependencyProperty H3ForegroundProperty;

        public Brush H3Foreground
        {
            get { return (Brush)GetValue(H3ForegroundProperty); }
            set { SetValue(H3ForegroundProperty, value); }
        }

        #endregion

        #region BlockQuoteProperties

        public static DependencyProperty BlockQuoteBackgroundProperty;

        public Brush BlockQuoteBackground
        {
            get { return (Brush)GetValue(BlockQuoteBackgroundProperty); }
            set { SetValue(BlockQuoteBackgroundProperty, value); }
        }

        #endregion

        internal List<RichTextBox> textBoxes = null;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            internalItemsControl = (ItemsControl)base.GetTemplateChild(HtmlTextBlock.PART_ItemsControl);

            if (!String.IsNullOrWhiteSpace(Html))
            {
                if (textBoxes == null || textBoxes.Count == 0)
                {
                    AppendHtml(Html);
                }
                else
                {
                    foreach (var rtb in textBoxes)
                    {
                        internalItemsControl.Items.Add(rtb);
                    }
                }
            }
        }

        #region Translation

        private void AppendHtml(string html)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            if (textBoxes == null)
                textBoxes = new List<RichTextBox>();
            textBoxes.Clear();
            internalItemsControl.Items.Clear();

            foreach (var node in htmlDoc.DocumentNode.ChildNodes)
            {
                if (node.Name.Equals("blockquote", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (var childNode in node.ChildNodes)
                    {
                        AppendRichtextBox(childNode);
                    }
                }
                else
                {
                    AppendRichtextBox(node);
                }
            }
        }

        private RichTextBox currentRtb;

        private void AppendRichtextBox(HtmlNode node)
        {
            RichTextBox rtb = new RichTextBox();
            rtb.Background = this.Background;
            rtb.FontFamily = this.FontFamily;
            rtb.FontSize = this.FontSize;
            rtb.FontStretch = this.FontStretch;
            rtb.FontStyle = this.FontStyle;
            rtb.FontWeight = this.FontWeight;

            textBoxes.Add(rtb);
            currentRtb = rtb;
            internalItemsControl.Items.Add(rtb);

            if (node.ParentNode.Name.Equals("blockquote", StringComparison.OrdinalIgnoreCase))
            {
                rtb.Background = BlockQuoteBackground;
                rtb.Padding = new Thickness(6);
                rtb.BorderThickness = new Thickness(0);
            }

            rtb.Margin = new Thickness(0);

            AppendParagraph(node, rtb);
        }

        private void AppendParagraph(HtmlNode node, RichTextBox rtb)
        {
            Paragraph paragraph = new Paragraph();
            rtb.Blocks.Add(paragraph);
            switch (node.Name.ToLower())
            {
                case "p":
                case "blockquote":
                case "div":
                    AppendChildren(node, paragraph, null);
                    break;
                default:
                    AppendFromHtml(node, paragraph, null);
                    break;
            }
        }

        private void AppendChildren(HtmlNode htmlNode, Paragraph paragraph, Span span)
        {
            foreach (var node in htmlNode.ChildNodes)
            {
                AppendFromHtml(node, paragraph, span);
            }
        }

        private void AppendFromHtml(HtmlNode node, Paragraph paragraph, Span span)
        {
            switch (node.Name.ToLower())
            {
                case "p":
                case "blockquote":
                    AppendSpan(node, paragraph, span, node.Name);
                    AppendLineBreak(node, paragraph, span, false);
                    break;
                case "h1":
                case "h2":
                case "h3":
                case "ul":
                    AppendSpan(node, paragraph, span, node.Name);
                    break;
                case "i":
                    AppendItalic(node, paragraph, span);
                    break;
                case "b":
                case "strong":
                    AppendBold(node, paragraph, span);
                    break;
                case "u":
                    AppendUnderline(node, paragraph, span);
                    break;
                case "#text":
                    AppendRun(node, paragraph, span);
                    break;
                case "a":
                    AppendHyperlink(node, paragraph, span);
                    break;
                case "li":
                    AppendRun(node, paragraph, span);
                    AppendSpan(node, paragraph, span, node.Name);
                    AppendLineBreak(node, paragraph, span, false);
                    break;
                case "br":
                    AppendLineBreak(node, paragraph, span, true);
                    break;
                case "image":
                case "img":
                    AppendImage(node, paragraph);
                    break;
                default:
                    Debug.WriteLine(String.Format("Element {0} not implemented", node.Name));
                    break;
            }
        }

        private void AppendLineBreak(HtmlNode node, Paragraph paragraph, Span span, bool traverse)
        {
            if (!node.ParentNode.Name.Equals("a"))
            {
                LineBreak lineBreak = new LineBreak();
                if (span != null)
                {
                    span.Inlines.Add(lineBreak);
                }
                else if (paragraph != null)
                {
                    paragraph.Inlines.Add(lineBreak);
                }
            }

            if (traverse)
                AppendChildren(node, paragraph, span);
        }

        private void AppendImage(HtmlNode node, Paragraph paragraph)
        {
            AppendLineBreak(node, paragraph, null, false);

            InlineUIContainer inlineContainer = new InlineUIContainer();
            Image image = new Image();
            if (node.Attributes["src"] != null)
            {
                try
                {
                    if (!node.Attributes["src"].Value.StartsWith("http://"))
                        return;

                    BitmapImage bitmap = new BitmapImage(new Uri(node.Attributes["src"].Value));
                    bitmap.CreateOptions = BitmapCreateOptions.None;
                    bitmap.ImageOpened += delegate
                    {
                        double bitmapWidth = bitmap.PixelWidth;
                        double actualWidth = currentRtb.ActualWidth;
                        image.Source = bitmap;
                        if (bitmapWidth < actualWidth)
                        {
                            image.Width = bitmapWidth;
                        }
                    };
                }
                catch (Exception)
                {
                    //fmmd MY CHANGE, stop crashing dammit!
                }
            }

            inlineContainer.Child = image;
            image.Stretch = Stretch.Uniform;

            paragraph.Inlines.Add(inlineContainer);

            AppendChildren(node, paragraph, null);
            AppendLineBreak(node, paragraph, null, false);
        }

        private void AppendHyperlink(HtmlNode node, Paragraph paragraph, Span span)
        {
            Hyperlink hyperlink = new Hyperlink();

            if (node.Attributes.Contains("href"))
            {
                string url = HttpUtility.HtmlDecode(node.Attributes["href"].Value);
                hyperlink.Command = NavigationCommand;
                hyperlink.CommandParameter = url;
                hyperlink.Click += navigationHandler;
            }

            if (HyperlinkFontFamily != null)
                hyperlink.FontFamily = HyperlinkFontFamily;
            else if (hyperlink.FontFamily != this.FontFamily)
                hyperlink.FontFamily = this.FontFamily;

            if (HyperlinkFontSize != null)
                hyperlink.FontSize = HyperlinkFontSize.Value;
            else if (hyperlink.FontSize != this.FontSize)
                hyperlink.FontSize = this.FontSize;

            if (HyperlinkFontStretch != null)
                hyperlink.FontStretch = HyperlinkFontStretch;
            else if (hyperlink.FontStretch != this.FontStretch)
                hyperlink.FontStretch = this.FontStretch;

            if (HyperlinkFontStyle != null)
                hyperlink.FontStyle = HyperlinkFontStyle;
            else if (hyperlink.FontStyle != this.FontStyle)
                hyperlink.FontStyle = this.FontStyle;

            if (HyperlinkFontWeight != null)
                hyperlink.FontWeight = HyperlinkFontWeight;
            else if (hyperlink.FontWeight != this.FontWeight)
                hyperlink.FontWeight = this.FontWeight;

            if (HyperlinkForeground != null)
                hyperlink.Foreground = HyperlinkForeground;
            else if (hyperlink.Foreground != this.Foreground)
                hyperlink.Foreground = this.Foreground;

            if (span != null)
            {
                span.Inlines.Add(hyperlink);
            }
            else if (paragraph != null)
            {
                paragraph.Inlines.Add(hyperlink);
            }

            AppendChildren(node, paragraph, hyperlink);
        }

        private void AppendSpan(HtmlNode node, Paragraph paragraph, Span span, string style)
        {
            Span span2 = new Span();

            switch (style.ToLower())
            {
                case "h1":
                    if (H1FontFamily != null)
                        span2.FontFamily = H1FontFamily;
                    else if (span2.FontFamily != this.FontFamily)
                        span2.FontFamily = this.FontFamily;

                    if (H1FontSize != null)
                        span2.FontSize = H1FontSize.Value;
                    else if (span2.FontSize != this.FontSize)
                        span2.FontSize = this.FontSize;

                    if (H1FontStretch != null)
                        span2.FontStretch = H1FontStretch;
                    else if (span2.FontStretch != this.FontStretch)
                        span2.FontStretch = this.FontStretch;

                    if (H1FontStyle != null)
                        span2.FontStyle = H1FontStyle;
                    else if (span2.FontStyle != this.FontStyle)
                        span2.FontStyle = this.FontStyle;

                    if (H1FontWeight != null)
                        span2.FontWeight = H1FontWeight;
                    else if (span2.FontWeight != this.FontWeight)
                        span2.FontWeight = this.FontWeight;

                    if (H1Foreground != null)
                        span2.Foreground = H1Foreground;
                    else if (span2.Foreground != this.Foreground)
                        span2.Foreground = this.Foreground;
                    break;

                case "h2":
                    if (H2FontFamily != null)
                        span2.FontFamily = H2FontFamily;
                    else if (span2.FontFamily != this.FontFamily)
                        span2.FontFamily = this.FontFamily;

                    if (H2FontSize != null)
                        span2.FontSize = H2FontSize.Value;
                    else if (span2.FontSize != this.FontSize)
                        span2.FontSize = this.FontSize;

                    if (H2FontStretch != null)
                        span2.FontStretch = H2FontStretch;
                    else if (span2.FontStretch != this.FontStretch)
                        span2.FontStretch = this.FontStretch;

                    if (H2FontStyle != null)
                        span2.FontStyle = H2FontStyle;
                    else if (span2.FontStyle != this.FontStyle)
                        span2.FontStyle = this.FontStyle;

                    if (H2FontWeight != null)
                        span2.FontWeight = H2FontWeight;
                    else if (span2.FontWeight != this.FontWeight)
                        span2.FontWeight = this.FontWeight;

                    if (H2Foreground != null)
                        span2.Foreground = H2Foreground;
                    else if (span2.Foreground != this.Foreground)
                        span2.Foreground = this.Foreground;
                    break;

                case "h3":
                    if (H3FontFamily != null)
                        span2.FontFamily = H3FontFamily;
                    else if (span2.FontFamily != this.FontFamily)
                        span2.FontFamily = this.FontFamily;

                    if (H3FontSize != null)
                        span2.FontSize = H3FontSize.Value;
                    else if (span2.FontSize != this.FontSize)
                        span2.FontSize = this.FontSize;

                    if (H3FontStretch != null)
                        span2.FontStretch = H3FontStretch;
                    else if (span2.FontStretch != this.FontStretch)
                        span2.FontStretch = this.FontStretch;

                    if (H3FontStyle != null)
                        span2.FontStyle = H3FontStyle;
                    else if (span2.FontStyle != this.FontStyle)
                        span2.FontStyle = this.FontStyle;

                    if (H3FontWeight != null)
                        span2.FontWeight = H3FontWeight;
                    else if (span2.FontWeight != this.FontWeight)
                        span2.FontWeight = this.FontWeight;

                    if (H3Foreground != null)
                        span2.Foreground = H3Foreground;
                    else if (span2.Foreground != this.Foreground)
                        span2.Foreground = this.Foreground;
                    break;
                default:
                    if (span2.FontFamily != this.FontFamily)
                        span2.FontFamily = this.FontFamily;

                    if (span2.FontSize != this.FontSize)
                        span2.FontSize = this.FontSize;

                    if (span2.FontStretch != this.FontStretch)
                        span2.FontStretch = this.FontStretch;

                    if (span2.FontStyle != this.FontStyle)
                        span2.FontStyle = this.FontStyle;

                    if (span2.FontWeight != this.FontWeight)
                        span2.FontWeight = this.FontWeight;

                    if (span2.Foreground != this.Foreground)
                        span2.Foreground = this.Foreground;
                    break;
            }

            if (span != null)
            {
                span.Inlines.Add(span2);
            }
            else if (paragraph != null)
            {
                paragraph.Inlines.Add(span2);
            }

            AppendChildren(node, paragraph, span2);
        }

        private void AppendBold(HtmlNode node, Paragraph paragraph, Span span)
        {
            Run run = new Run();
            run.FontWeight = FontWeights.Bold;
            if (run.FontFamily != this.FontFamily)
                run.FontFamily = this.FontFamily;

            if (run.FontSize != this.FontSize)
                run.FontSize = this.FontSize;

            if (run.FontStretch != this.FontStretch)
                run.FontStretch = this.FontStretch;

            if (run.FontStyle != this.FontStyle)
                run.FontStyle = this.FontStyle;

            if (run.Foreground != this.Foreground)
                run.Foreground = this.Foreground;

            if (span != null)
            {
                span.Inlines.Add(run);
            }
            else if (paragraph != null)
            {
                paragraph.Inlines.Add(run);
            }

            AppendChildren(node, paragraph, span);
        }

        private void AppendItalic(HtmlNode node, Paragraph paragraph, Span span)
        {
            Run run = new Run();
            run.FontStyle = FontStyles.Italic;

            if (run.FontFamily != this.FontFamily)
                run.FontFamily = this.FontFamily;

            if (run.FontSize != this.FontSize)
                run.FontSize = this.FontSize;

            if (run.FontStretch != this.FontStretch)
                run.FontStretch = this.FontStretch;

            if (run.FontWeight != this.FontWeight)
                run.FontWeight = this.FontWeight;

            if (run.Foreground != this.Foreground)
                run.Foreground = this.Foreground;

            if (span != null)
            {
                span.Inlines.Add(run);
            }
            else if (paragraph != null)
            {
                paragraph.Inlines.Add(run);
            }

            AppendChildren(node, paragraph, span);
        }

        private void AppendUnderline(HtmlNode node, Paragraph paragraph, Span span)
        {
            Run run = new Run();
            run.TextDecorations = TextDecorations.Underline;

            if (run.FontStyle != this.FontStyle)
                run.FontStyle = this.FontStyle;

            if (run.FontFamily != this.FontFamily)
                run.FontFamily = this.FontFamily;

            if (run.FontSize != this.FontSize)
                run.FontSize = this.FontSize;

            if (run.FontStretch != this.FontStretch)
                run.FontStretch = this.FontStretch;

            if (run.FontWeight != this.FontWeight)
                run.FontWeight = this.FontWeight;

            if (run.Foreground != this.Foreground)
                run.Foreground = this.Foreground;

            if (span != null)
            {
                span.Inlines.Add(run);
            }
            else if (paragraph != null)
            {
                paragraph.Inlines.Add(run);
            }

            AppendChildren(node, paragraph, span);
        }

        private void AppendRun(HtmlNode node, Paragraph paragraph, Span span)
        {
            Run run = new Run();

            if (node.Name.Equals("li", StringComparison.OrdinalIgnoreCase))
            {
                run.Text = "•";
            }
            else
            {
                run.Text = DecodeAndCleanupHtml(node.InnerText);

            }

            if (span != null)
            {
                span.Inlines.Add(run);
            }
            else if (paragraph != null)
            {
                paragraph.Inlines.Add(run);
            }
        }


        private string DecodeAndCleanupHtml(string html)
        {
            // this breaks the designer somehow
            StringBuilder builder = new StringBuilder();
            builder.Append(HttpUtility.HtmlDecode(html));
            builder.Replace("&nbsp;", " ");

            return builder.ToString();
        }

        #endregion
    }
}

namespace Krempel.WP7.Core.Controls
{
    public static class HtmlTextBlockExtensions
    {
        public static IEnumerable<Inline> GetChildInlines(this RichTextBox richTextBox)
        {
            foreach (var block in richTextBox.Blocks.OfType<Paragraph>())
            {
                foreach (var inline in block.GetChildInlines())
                {
                    yield return inline;
                }
            }
        }

        public static IEnumerable<Inline> GetChildInlines(this Paragraph paragraph)
        {
            foreach (var inline in paragraph.Inlines)
            {
                yield return inline;

                if (inline is Span)
                {
                    foreach (var subInline in ((Span)inline).GetChildInlines())
                    {
                        yield return subInline;
                    }
                }
            }
        }

        public static IEnumerable<Inline> GetChildInlines(this Span span)
        {
            foreach (var inline in span.Inlines)
            {
                yield return inline;

                if (inline is Span)
                {
                    foreach (var subInline in ((Span)inline).GetChildInlines())
                    {
                        yield return subInline;
                    }
                }
            }
        }
    }
}
