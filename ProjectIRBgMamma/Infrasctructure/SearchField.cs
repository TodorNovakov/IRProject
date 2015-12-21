
namespace ProjectIRBgMamma.Infrasctructure
{
    /// <summary>
    /// Custom attribute to define the field that can be seached on
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Field | System.AttributeTargets.Property)]
    public class SearchField : System.Attribute
    {
        public string[] CombinedSearchFields;


        public SearchField(params string[] values)
        {
            this.CombinedSearchFields = values;
        }
    }
}