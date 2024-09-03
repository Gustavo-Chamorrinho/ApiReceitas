namespace ApiReceitas.Models
{
    public class Receitas
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public List<string> Ingredientes { get; set; } = new List<string>();
        public string Instrucoes {  get; set; } = string.Empty;
        public string UrlImagem { get; set; } = string.Empty;

    }
}
