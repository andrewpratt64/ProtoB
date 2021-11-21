using ProtoB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoB.ViewModels
{
    public class EntityViewModel
        : ViewModelBase,
        IDisposable
    {
        /// <summary>Entity this viewmodel encapsulates</summary>
        private unsafe EntityModel* m_model;

        /// <summary>Root subentity of this entity</summary>
        private SubentityViewModel m_rootSubentity;


        /// <summary>Name of entity</summary>
        public string Name
        {
            get
            {
                unsafe { return new string(m_model->name); }
            }
            set
            {
                unsafe
                {
                    Native.Util.AssignStringToCharArray(
                        m_model->name,
                        Native.Util.ENTITY_MODEL_NAME_MAXLEN,
                        value
                    );
                }
            }
        }

        public int Subtype
        {
            get
            {
                unsafe { return m_model->subtype; }
            }
            set
            {
                unsafe { m_model->subtype = value; }
            }
        }

        /// <summary>Get the root subentity of this entity</summary>
        public SubentityViewModel RootSubentity
        {
            get { return m_rootSubentity; }
        }


        public unsafe EntityViewModel(EntityModel* model)
        {
            m_model = model;
            m_rootSubentity = new SubentityViewModel(m_model->rootSubentity);
        }


        public void Dispose()
        {
            unsafe
            {
                Native.ProtoBCpp.destroyUnmanagedEntityModel(m_model);
            }

            GC.SuppressFinalize(this);
        }
    }
}
