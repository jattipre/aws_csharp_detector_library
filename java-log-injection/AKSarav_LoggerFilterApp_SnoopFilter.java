package com.logger.app;
import java.io.IOException;

import javax.servlet.*;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import org.apache.log4j.Logger;



public class SnoopFilter implements javax.servlet.Filter  {

	@Override
	public void destroy() {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void doFilter(ServletRequest req, ServletResponse resp, FilterChain chain)
			throws IOException, ServletException {
		
		HttpServletRequest request = (HttpServletRequest) req;
		
		
		System.out.println("DEBUG "+this.getClass());
		
		//Initialize the com.logger.app.Home logger
		Logger logger = Logger.getLogger(this.getClass());
		
		logger.info(this.getClass()+" has been called by the SESSION ID"+request.getRequestedSessionId());
		
		logger.debug("======================================");
		logger.debug("REQUEST INFO");
		logger.debug("======================================");
		logger.debug("SESSIONID:"+request.getRequestedSessionId());
		logger.debug("QUERYSTRING:"+request.getQueryString());
		logger.debug("INCOMING URL:"+request.getRequestURL());
		logger.debug("SERVLET CONTEXT:"+request.getServletContext());
		logger.debug("======================================");
		
		
		System.out.println("FILTER IS ON");
		
	   chain.doFilter(req, resp);
	}

	@Override
	public void init(FilterConfig arg0) throws ServletException {
		// TODO Auto-generated method stub
		
	}
	
	
	
}
