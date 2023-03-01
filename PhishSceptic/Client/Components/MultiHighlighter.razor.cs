using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using MudBlazor;
using MudBlazor.Components.Highlighter;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace PhishSceptic.Client.Components
{
    public partial class MultiHighlighter : MudComponentBase
    {

        private Memory<string> _fragments;
        private string _regex;

        private string[] _regexes;

        [Parameter] public string[] Colors { get; set; }

        /// <summary>
        /// The whole text in which a fragment will be highlighted
        /// </summary>
        [Parameter]
        [Category(CategoryTypes.Highlighter.Behavior)]
        public string Text { get; set; }

        /// <summary>
        /// The fragment of text to be highlighted
        /// </summary>
        [Parameter]
        [Category(CategoryTypes.Highlighter.Behavior)]
        public string HighlightedText { get; set; }

        /// <summary>
        /// The fragments of text to be highlighted
        /// </summary>
        [Parameter]
        [Category(CategoryTypes.Highlighter.Behavior)]
        public IEnumerable<string> HighlightedTexts { get; set; }

        /// <summary>
        /// Whether or not the highlighted text is case sensitive
        /// </summary>
        [Parameter]
        [Category(CategoryTypes.Highlighter.Behavior)]
        public bool CaseSensitive { get; set; }

        /// <summary>
        /// If true, highlights the text until the next regex boundary
        /// </summary>
        [Parameter]
        [Category(CategoryTypes.Highlighter.Behavior)]
        public bool UntilNextBoundary { get; set; }

        //TODO
        //Accept regex highlightings
        // [Parameter] public bool IsRegex { get; set; }

        protected override void OnParametersSet()
        {
            //would have to have multiple regexes
            _fragments = Splitter.GetFragments(Text, HighlightedText, HighlightedTexts, out _regex, CaseSensitive, UntilNextBoundary);
            Console.WriteLine(_regex);
            _regexes = _regex.Split("|");
            for (int i= 0; i < _regexes.Length; i++)
            {
                _regexes[i] = _regexes[i].Trim(new Char[] { '(', '?', ':', ')' });
            }
        }

        /*
        protected override void BuildRenderTree(RenderTreeBuilder __builder)
        {
            Span<string> span = _fragments.Span;
            foreach (string text in span)
            {
                if (!string.IsNullOrWhiteSpace(_regex) && Regex.IsMatch(text, _regex, (!CaseSensitive) ? RegexOptions.IgnoreCase : RegexOptions.None))
                {
                    __builder.OpenElement(0, "mark");
                    __builder.AddAttribute(1, "class", base.Class);
                    __builder.AddAttribute(2, "style", base.Style);
                    __builder.AddMultipleAttributes(3, RuntimeHelpers.TypeCheck((IEnumerable<KeyValuePair<string, object>>)base.UserAttributes));
                    __builder.AddContent(4, text);
                    __builder.CloseElement();
                }
                else
                {
                    __builder.AddContent(5, text);
                }
            }
        }
        */

    }
}
