using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Core_MVC_without_EF.Data;
using Core_MVC_without_EF.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Core_MVC_without_EF.Controllers
{
    public class MovieController : Controller
    {
        private readonly IConfiguration _configuration;

        public MovieController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        // GET: Movie
        public IActionResult Index()
        {
            DataTable dtbl = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("MovieViewAll", sqlConnection);  // stored procedure ja yhteys parametreinä
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDa.Fill(dtbl);
            }
            return View(dtbl);
        }


        // GET: Movie/AddOrEdit/
        public IActionResult AddOrEdit(int? id)
        {
            MovieViewModel movieViewModel = new MovieViewModel();
            if (id > 0) { 
                movieViewModel = FetchMovieByID(id);                    // FetchMovieByID funktio sivun alalaidassa, jos muokataan vanhaa eikä tehdä uutta
            }
            return View(movieViewModel);
        }

        // POST: Movie/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOrEdit(int id, [Bind("MovieID,Title,Director,Rating")] MovieViewModel movieViewModel)
        {

            if (ModelState.IsValid)
            {
                using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))        // parametrina tietokantayhteys ja DevConnection string appsettings tiedostosta
                {
                    sqlConnection.Open();
                    SqlCommand sqlCmd = new SqlCommand("MovieAddOrEdit", sqlConnection);
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("MovieID", movieViewModel.MovieID);
                    sqlCmd.Parameters.AddWithValue("Title", movieViewModel.Title);
                    sqlCmd.Parameters.AddWithValue("Director", movieViewModel.Director);
                    sqlCmd.Parameters.AddWithValue("Rating", movieViewModel.Rating);
                    sqlCmd.ExecuteNonQuery();
                } // A Using block guarantees the disposal
                return RedirectToAction(nameof(Index));
            }
            return View(movieViewModel);
        }

        // GET: Movie/Delete/5
        public IActionResult Delete(int? id)
        {
            MovieViewModel movieViewModel = FetchMovieByID(id);
            return View(movieViewModel);
        }

        // POST: Movie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                SqlCommand sqlCmd = new SqlCommand("MovieDeleteByID", sqlConnection);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("MovieID", id);
                sqlCmd.ExecuteNonQuery();
            }
            return RedirectToAction(nameof(Index));
        }

        [NonAction]
        public MovieViewModel FetchMovieByID(int? id)
        {
            MovieViewModel movieViewModel = new MovieViewModel();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                DataTable dtbl = new DataTable();
                sqlConnection.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("MovieViewByID", sqlConnection);   // SQL kannan stored procedure
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDa.SelectCommand.Parameters.AddWithValue("MovieID", id);
                sqlDa.Fill(dtbl);
                if (dtbl.Rows.Count == 1)
                {
                    movieViewModel.MovieID = Convert.ToInt32(dtbl.Rows[0]["MovieID"].ToString());
                    movieViewModel.Title = dtbl.Rows[0]["Title"].ToString();
                    movieViewModel.Director = dtbl.Rows[0]["Director"].ToString();
                    movieViewModel.Rating = Convert.ToInt32(dtbl.Rows[0]["Rating"].ToString());
                }
                return movieViewModel;
            }
        }

    }
}
