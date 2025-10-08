// Author Michael Meier micmine4@gmail.com
package net.wfs.web;

import jptools.logger.Logger;
import net.wfs.web.engine.db.DatabaseConnectorFactory;

import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;
import java.io.IOException;
import java.io.PrintWriter;
import java.sql.SQLException;

public class GroupServlet extends HttpServlet {

    private static Logger log = Logger.getLogger(GroupServlet.class);

    /**
     * Accounmanagment create / delete accounts
     *
     * @param request
     * @param response
     * @throws IOException
     */

    public void doPost(HttpServletRequest request, HttpServletResponse response)
            throws IOException {

        PrintWriter out = response.getWriter();
        HttpSession session = request.getSession();

        String type = request.getParameter("type");
        String name = request.getParameter("name");
        String id = request.getParameter("id");

        if (session.getAttribute(LoginServlet.RANK_ATTRIBUTE_NAME).equals(Rank.Teacher) ||
                session.getAttribute(LoginServlet.RANK_ATTRIBUTE_NAME).equals(Rank.Admin)) {


            if (type == null) {


            } else if (type.equals("create")) {
                try {

                    String idnew = String.valueOf(1 + Integer.parseInt(String.valueOf(DatabaseConnectorFactory.getInstance().getGroupDatabaseConnector().countGroup())));

                    out.print("Add Group ...");

                    DatabaseConnectorFactory.getInstance().getGroupDatabaseConnector().writeName(idnew, name);
                    DatabaseConnectorFactory.getInstance().getGroupDatabaseConnector().writeSize(idnew, "0");


                    out.print("done");


                    log.debug("Created an Group name : " + name);

                    response.sendRedirect("list");

                } catch (SQLException e) {
                    e.printStackTrace();
                }

            } else if (type.equals("delete")) {
                try {


                    out.print("Deleting Group ...");

                    out.print("(" + id + ")");

                    DatabaseConnectorFactory.getInstance().getGroupDatabaseConnector().dorpGroup(id);


                    out.print("done");


                    log.debug("Drop an Group id : " + id);

                    response.sendRedirect("list");

                } catch (SQLException e) {
                    e.printStackTrace();
                }
            } else {
                log.error("Link error : " + request.getRequestURI());

            }
        }
    }



    public static int safeLongToInt(long l) {
        int i = (int) l;
        if ((long) i != l) {
            throw new IllegalArgumentException(l + " cannot be cast to int without changing its value.");
        }
        return i;
    }
}
