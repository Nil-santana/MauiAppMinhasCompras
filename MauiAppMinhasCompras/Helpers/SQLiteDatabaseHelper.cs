using MauiAppMinhasCompras.Models;
using MauiAppMinhasCompras.Views;
using SQLite;

namespace MauiAppMinhasCompras.Helpers
{
    public class SQLiteDatabaseHelper
    {

        readonly SQLiteAsyncConnection _conn;

        public SQLiteDatabaseHelper(string path)
        {

            _conn = new SQLiteAsyncConnection(path);
            _conn.CreateTableAsync<Produto>().Wait();   
        }
        public Task <int> Insert (Produto p)
        {
            return _conn.InsertAsync(p);
        }

        public Task<List<Produto>> Update(Produto p) 
        {
            string sql = "UPDATE Produto SET Descricao=?, " +
                         "Quantidade=?, Preco=?, Categoria=? WHERE Id=?";
                         return _conn.QueryAsync<Produto>(
                         sql, p.Descricao, p.Quantidade, p.Preco, p.Categoria, p.Id);

        }

        public Task<int> Delete(int id) 
        {
            return _conn.Table<Produto>().DeleteAsync(i => i.Id == id);
        }

        public Task<List<Produto>> GetAll() 
        {

            return _conn.Table<Produto>().ToListAsync();
        }
        public Task<List<Produto>> GetByCategoria(String categoria) 
        {
            return _conn.Table<Produto>().Where(i => i.Categoria == categoria).ToListAsync();
        }


        public Task<List<Produto>> Search(string q)
        {
            string sql = "SELECT * FROM Produto WHERE Descricao LIKE '%" + q + "%'";
            return _conn.QueryAsync<Produto>(sql);
        }

        internal async Task<List<RelatorioItem>> GetTotalPorCategoria()
        {
            var todos = await _conn.Table<Produto>().ToListAsync();
            return todos.GroupBy(p =>p.Categoria)
                .Select (g => new RelatorioItem { Categoria = g.Key ?? "Outros", 
                    Total = g.Sum(p=> p.Total), Quantidade = g.Count () })
                .OrderByDescending( r => r.Total) .ToList();    
            
        }
    }
    public class RelatorioItem
    {
        public string Categoria { get; set; }
        public double Total { get; set; }
        public int Quantidade { get; set; }
    }
}
