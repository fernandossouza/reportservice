using System.Collections.Generic;

namespace reportservice.Model{
    public class RelatorioAlarm {        

        public List<AlarmFront> graphs;
        public List<TabelaFront> table;

        public RelatorioAlarm(List<AlarmFront> graphs, List<TabelaFront> table){
            this.graphs = graphs;
            this.table = table;
        }
    }
} 