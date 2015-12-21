using System.Linq;
using System.Reflection;
using System.Text;
using Lucene.Net.Documents;

namespace ProjectIRBgMamma.Infrasctructure
{

    /// <summary>
    /// Abstract class as base for all Seach Documents
    /// </summary>
    public abstract class ADocument : IDocument
    {
        private int id;

        /// <summary>
        /// The ID of the item
        /// </summary>
        [SearchField]
        public int Id
        {
            set
            {
                id = value;
                AddParameterToDocument("Id", id, Field.Store.YES, Field.Index.NOT_ANALYZED);
            }
            get { return id; }
        }

        private readonly Document document;

        /// <summary>
        /// The Lucene Document
        /// </summary>
        public Document Document { get { return document; } }

        /// <summary>
        /// Constructor
        /// </summary>
        protected ADocument()
        {
            document = new Document();
        }


        /// <summary>
        /// Method to add parameters to the Lucene Document
        /// Only parameters that are added to the Lucene document can be searched on
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <param name="value">The value of the parameter</param>
        /// <param name="store">The Store setting</param>
        /// <param name="index">The Index setting</param>
        private void AddParameterToDocument(string name, dynamic value, Field.Store store, Field.Index index)
        {
            document.Add(new Field(name, value.ToString(), store, index));
        }

        protected void AddParameterToDocumentStoreParameter(string name, dynamic value)
        {
            AddParameterToDocument(name, value, Field.Store.YES, Field.Index.ANALYZED);
        }

        protected void AddParameterToDocumentNoStoreParameter(string name, dynamic value)
        {
            AddParameterToDocument(name, value, Field.Store.NO, Field.Index.ANALYZED);
        }
    }
}