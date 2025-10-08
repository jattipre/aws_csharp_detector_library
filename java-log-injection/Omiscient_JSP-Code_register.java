import java.io.IOException;
import java.io.PrintWriter;
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.PreparedStatement;
import java.sql.SQLException;
import java.util.logging.Level;
import java.util.logging.Logger;
import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

public class register extends HttpServlet{
    
              static String driver = "com.mysql.jdbc.Driver";
              static String url = "jdbc:mysql://localhost:3306/";
              static String dbname = "omniscient";

   
    @Override
    public void doPost(HttpServletRequest request,HttpServletResponse response) throws IOException,ServletException{
        
      
        
        
          response.setContentType("text/html");
        
          PrintWriter out = response.getWriter();
       
          Connection conn = null;
          
          String user = request.getParameter("register_mode");
          if(user.equalsIgnoreCase("student"))
          {
              
              String USN = request.getParameter("USN");
              String Name = request.getParameter("StudName");
              String Email = request.getParameter("email");
              String Password = request.getParameter("Password");
              System.out.println(USN);
              
         
                 
          }
                  try {
            Class.forName("apache_derby_net");
            conn = DriverManager.getConnection("jdbc:derby://localhost:1527/abc","abcd","password");
            
             out.println("<html>"
                      + "<head>"
                      + "student"
                      + "</head>"
                      + "<body>"
                      + "<div>"
                      + "done connecting"+""
                      + "</div>"
                      +"</body>"
                      +"</html>");
                  }
                  catch(Exception ex){
                      
                  }
       
          }
    }

                  
                   
          
          
//          else if(user.equalsIgnoreCase("staff"))
//          {
//              String USN = request.getParameter("USN");
//              String Name = request.getParameter("StudName");
//              String Password = request.getParameter("Password");
//               
//              RequestDispatcher rs = request.getRequestDispatcher("register_staff.html");
//              rs.include(request, response);
//              
//          }
//        }
//    
        
          
    

    
