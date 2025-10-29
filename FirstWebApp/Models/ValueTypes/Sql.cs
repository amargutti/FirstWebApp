namespace FirstWebApp.Models.ValueTypes
{
    public class Sql
    {
            private Sql(string value)
            {
                Value = value;
            }
            //Proprietà per conservare il valore originale
            public string Value { get; }

            //Conversione da/per il tipo string
            public static explicit operator Sql(string value) => new Sql(value);
            public override string ToString()
            {
                return this.Value;
            }
    }
}
