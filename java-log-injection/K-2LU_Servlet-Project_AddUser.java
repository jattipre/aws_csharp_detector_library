package cscorner;

import java.io.IOException;
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;

import javax.servlet.RequestDispatcher;
import javax.servlet.ServletException;
import javax.servlet.annotation.WebServlet;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import com.mysql.cj.xdevapi.Statement;

/**
 * Servlet implementation class NewUserServlet
 */
@WebServlet("/NewUserServlet")
public class NewUserServlet extends HttpServlet {

	Connection con;
	PreparedStatement ps;
	ResultSet rs;
	protected void doPost(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
		// TODO Auto-generated method stub
//		doGet(request, response);
		;
		
		try {
			String sql = "INSERT INTO LoginTable(uname, passwd, userType)" + "VALUES(?, ?, ?)";
			Class.forName("com.mysql.cj.jdbc.Driver");		
			con = DriverManager.getConnection("jdbc:mysql://localhost:3306/login", "root", "admin");
			
			String n = request.getParameter("name");
			String p = request.getParameter("pass");
			
			System.out.println(n+" "+p);
			
			ps = con.prepareStatement("INSERT INTO LoginTable(uname, passwd, userType) VALUES(?, ?, ?)");
			ps.setString(1, n);
			ps.setString(2, p);
			ps.setString(3, "Student");
			
			ps.executeUpdate();
			
			RequestDispatcher rd = request.getRequestDispatcher("Admin.jsp");
			rd.forward(request, response);
			
		} catch (ClassNotFoundException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (SQLException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}finally {
		    if (rs != null) {
		        try {
		            rs.close();
		        } catch (SQLException e) { /* Ignored */}
		    }
		    if (ps != null) {
		        try {
		            ps.close();
		        } catch (SQLException e) { /* Ignored */}
		    }
		    if (con != null) {
		        try {
		            con.close();
		        } catch (SQLException e) { /* Ignored */}
		    }
		}
	}

}
