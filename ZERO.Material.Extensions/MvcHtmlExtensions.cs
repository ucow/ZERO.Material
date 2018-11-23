using System.Linq.Expressions;

namespace System.Web.Mvc.Html
{
    public static class MvcHtmlExtensions
    {
        public static MvcHtmlString TableFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression)
        {
            return new MvcHtmlString();
        }
    }
}