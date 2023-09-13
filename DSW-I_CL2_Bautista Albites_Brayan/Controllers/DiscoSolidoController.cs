using DSW_I_CL2_Bautista_Albites_Brayan.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace CL1LaboPeruSAC.Controllers
{
    public class DiscoSolidoController : Controller
    {
        public readonly IConfiguration _config;

        //creamos un constructor...
        public DiscoSolidoController(IConfiguration IConfig)
        {
            _config = IConfig;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Index", "VentasHardware");
            //return View();
        }  //fin del metodo...

        public IActionResult DiscoSolido()
        {
            return View();
        }  //fin del metodo...

        public IActionResult CalcularCds(ClassCalcular objcalcular)
        {
            string mensaje = "";
            //pequeño algoritmo...
            int Gyga = 1024; // Equivalente de 1 GB en MB

            int espacioTotalEnMB = objcalcular.CapaDiscoGB * Gyga;
            objcalcular.TotalCds = (int)Math.Ceiling(espacioTotalEnMB / (double)objcalcular.CapaCdMB);

            //insertar a la base de datos...
            using (SqlConnection cn = new SqlConnection(_config["ConnectionStrings:cn"]))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_guardar_cds", cn);

                    //aperturamos la base de datos
                    cn.Open();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@cli", objcalcular.Cliente);
                    cmd.Parameters.AddWithValue("@cad", objcalcular.CapaDiscoGB);
                    cmd.Parameters.AddWithValue("@cac", objcalcular.CapaCdMB);
                    cmd.Parameters.AddWithValue("@tot", objcalcular.TotalCds);

                    //realizamos la ejecucion del procedimiento almacenado
                    int c = cmd.ExecuteNonQuery();
                    mensaje = $"registro insertado {c} vendedor";

                }
                catch (Exception ex)
                {
                    mensaje = ex.Message;
                }   //fin del catch...

            }   //fin del using...
            ViewBag.Mensaje = mensaje;

            //retornar a la vista...
            return RedirectToAction("listadoCalculos", "DiscoSolido");
        }   //fin del metodo...

        //******************* codigo para editar listar los calculos...
        IEnumerable<ClassCalcular> calculos ()
        {
            List<ClassCalcular> calculos = new List<ClassCalcular>();
            using (SqlConnection cn = new SqlConnection(_config["ConnectionStrings:cn"]))
            {
                //aperturamos la base de datos...
                cn.Open();
                //aplicamos el sqlcommnad....
                SqlCommand cmd = new SqlCommand("sp_listar_cds", cn);
                //aplicamos el sqldatareader...
                SqlDataReader dr = cmd.ExecuteReader();
                //aplicamos un bucle...
                while (dr.Read())
                {
                    calculos.Add(new ClassCalcular()
                    {
                        Idcd = dr.GetInt32(0),
                        Cliente = dr.GetString(1),
                        CapaDiscoGB = dr.GetInt32(2),
                        CapaCdMB = dr.GetInt32(3),
                        TotalCds = dr.GetInt32(4)
                    });
                }   //fin del bucle while...
            }   //fin del using...
            
            //retornamos la data recuperada de la base de datos...
            return calculos;

        }   //fin del metodo listado de calculos...

        //metodo que retorna el listado de cotizaciones registrado en base de datos...
        public async Task<IActionResult> listadoCalculos(int p)
        {
            //declaramos  variables para realizar la paginacion....
            //numero de registros
            int nr = 5;
            //total de registros....
            int tr = calculos().Count();
            int paginas = nr % tr > 0 ? tr / nr + 1 : tr / nr;
            //enviamos la paginas a la vista...
            ViewBag.paginas = paginas;

            //retornamos listado a la vista...
            return View(await Task.Run(() => calculos().Skip(p * nr).Take(nr)));

        }   //fin del metodo listado de calculos...

        //******************* codigo para editar...
        [HttpGet]
        public IActionResult Edit(int id)
        {
            //instanciamos la respectiva clase...
            ClassCalcular cal = new ClassCalcular();
            //obtenemos la conexion 
            using (SqlConnection cn = new SqlConnection(_config["ConnectionStrings:cn"]))
            {
                //invocamos al procedimiento almacenado
                SqlCommand cmd = new SqlCommand("sp_buscar_cds @cod", cn);
                // cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cod", id);
                //aperturamos la base de datos
                cn.Open();
                //recuperamos los datos de la base de datos y los almacenamos en la base de datos
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    //aplicamos una condicion
                    if (dr.Read())
                    {
                        cal.Idcd = Convert.ToInt32(dr["Idcd"]);
                        cal.Cliente = (string)dr["cliente"];
                        cal.CapaDiscoGB = (int)dr["CapaDisco"];
                        cal.CapaCdMB = (int)dr["CapaCds"];
                        cal.TotalCds = (int)dr["totalCds"];
                    }   //fin de la condicion...
                }   //fin del using...
                return View(cal);
            }   //fin del using...
        }   //fin del metodo edit GET...

        [HttpPost, ActionName("Edit")]
        public IActionResult Edit_Post (ClassCalcular ObjCalcular)
        {
            string mensaje = "";
            //pequeño algoritmo...
            int Gyga = 1024; // Equivalente de 1 GB en MB

            int espacioTotalEnMB = ObjCalcular.CapaDiscoGB * Gyga;
            ObjCalcular.TotalCds = (int)Math.Ceiling(espacioTotalEnMB / (double)ObjCalcular.CapaCdMB);

            using (SqlConnection cn = new SqlConnection(_config["ConnectionStrings:cn"]))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_actualizar_cds", cn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    //aperturamos la base de datos
                    cn.Open();
                    cmd.Parameters.AddWithValue("@cod", ObjCalcular.Idcd);
                    cmd.Parameters.AddWithValue("@cli", ObjCalcular.Cliente);
                    cmd.Parameters.AddWithValue("@cad", ObjCalcular.CapaDiscoGB);
                    cmd.Parameters.AddWithValue("@cac", ObjCalcular.CapaCdMB);
                    cmd.Parameters.AddWithValue("@tot", ObjCalcular.TotalCds);

                    //realizamos la ejecucion...
                    int c = cmd.ExecuteNonQuery();
                    mensaje = $"registro actualizado {c} en calculos";

                } catch (Exception ex)
                {
                    mensaje += ex.Message;
                }   //fin del catch...

            }   //fin del using...

            //enviamos a la vista el mensaje
            ViewBag.mensaje = mensaje;

            //redirecciona hacia el listado
            return RedirectToAction("listadoCalculos");
        }   //fin del metodo edit post...

        //******************* codigo para eliminar con plantilla...
        /*
        [HttpGet]
        public IActionResult Delete(int id)
        {
            //instanciamos la respectiva clase...
            ClassCalcular cal = new ClassCalcular();
            //obtenemos la conexion...
            using (SqlConnection cn = new SqlConnection(_config["ConnectionStrings:cn"]))
            {
                //invocamos al procedimiento almacenado
                SqlCommand cmd = new SqlCommand("sp_buscar_cds @cod", cn);
                // cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cod", id);
                //aperturamos la base de datos
                cn.Open();
                //recuperamos los datos de la base de datos y los almacenamos en las
                //propiedades
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    //aplicamos una condicion
                    if (dr.Read())
                    {
                        cal.Idcd = Convert.ToInt32(dr["Idcd"]);
                        cal.Cliente = (string)dr["cliente"];
                        cal.CapaDiscoGB = (int)dr["CapaDisco"];
                        cal.CapaCdMB = (int)dr["CapaCds"];
                        cal.TotalCds = (int)dr["totalCds"];
                    }  //fin de la condicion...

                }

            }   //fin del using....
            return View(cal);
        }   //fin del metodo delete...
        */
       // [HttpPost, ActionName("Delete")]
        [HttpGet, ActionName("Delete")]
        public IActionResult DeleteCoti(int id)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(_config["ConnectionStrings:cn"]))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_eliminar_cds", cn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    //abrimos la base de datos
                    cn.Open();
                    cmd.Parameters.AddWithValue("@cod", id);
                    //realizamos la ejecucion del procedimiento
                    int c = cmd.ExecuteNonQuery();
                    mensaje = $"registro eliminado {c} de los calculos";
                }
                catch (Exception ex)
                {
                    mensaje = ex.Message;
                }//fin del catch...
            }   //fin del using...
            //enviamos el mensaje hacia la vista
            ViewBag.mensaje = mensaje;
            //redireccionamos hacia el listado
            return RedirectToAction("listadoCalculos");

        }  //fin del metodo elminar POST..
    }   //fin de la clase controlador...
}   //fin del namespace...
