namespace VoxPopuliLibrary.client.config
{
    public interface ISetting
    {
        object GetData();
        void SetData(object value);
        void OnDataUpdated();
    }

    public abstract class Setting<T> : ISetting
    {
        protected T data;

        public object GetData()
        {
            return data;
        }

        public void SetData(object value)
        {
            Data = (T)value;
        }

        public T Data
        {
            get { return data; }
            set
            {
                if (!EqualityComparer<T>.Default.Equals(data, value))
                {
                    data = value;
                    OnDataUpdated();
                }
            }
        }

        public abstract void OnDataUpdated();
    }
    public class BoolSetting : Setting<bool>
    {
        public override void OnDataUpdated()
        {
            // Actions spécifiques à un booléen lors de la mise à jour des données
        }
    }

    public class StringSetting : Setting<string>
    {
        public override void OnDataUpdated()
        {
            // Actions spécifiques à une chaîne de caractères lors de la mise à jour des données
        }
    }

    public class IntSetting : Setting<int>
    {
        public override void OnDataUpdated()
        {
            // Actions spécifiques à un entier lors de la mise à jour des données
        }
    }

    public class FloatSetting : Setting<float>
    {
        public override void OnDataUpdated()
        {
            // Actions spécifiques à un nombre à virgule flottante lors de la mise à jour des données
        }
    }

}
