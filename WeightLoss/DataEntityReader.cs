using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace DataEntityReader
{
    public class EntityDataSourceReader<T> where T : class
    {
        private DataSourceViewSelectCallback myDelegate = null;
        private System.Web.UI.WebControls.EntityDataSource _source = null;
        List<T> dataList = new List<T>();

        public EntityDataSourceReader(System.Web.UI.WebControls.EntityDataSource source)
        {
            myDelegate = new DataSourceViewSelectCallback(AllDone);
            _source = source;
        }



        protected void AllDone(IEnumerable data)
        {
            foreach (var v in data)
                dataList.Add(GetItemObject<T>(v));
        }

        public List<T> GetData()
        {
            Control ctl = _source as Control;

            if (ctl != null)
            {
                IDataSource datasrc = ctl as IDataSource;
                if (datasrc != null)
                {
                    string viewName = datasrc.GetViewNames().Cast<string>().First();
                    DataSourceView view = datasrc.GetView(viewName);
                    view.Select(new DataSourceSelectArguments(), myDelegate);

                    return dataList;
                }
            }

            return null;
        }


        public static TEntity GetItemObject<TEntity>(object dataItem) where TEntity : class
        {
            var entity = dataItem as TEntity;
            if (entity != null)
            {
                return entity;
            }
            var td = dataItem as ICustomTypeDescriptor;
            if (td != null)
            {
                return (TEntity)td.GetPropertyOwner(null);
            }
            return null;
        }

    }
}