namespace reportservice.Model.Genealogy
{
    public class Aco{
        public Aco(string numeroRolo, string qtd, string lote, long data){
            this.numeroRolo = numeroRolo;
            this.qtd = qtd;
            this.lote = lote;
            this.data = data;
        }
        public string numeroRolo {get; set;}
        public string qtd {get; set;}
        public string lote {get; set;}
        public long data {get; set;}
    }
}