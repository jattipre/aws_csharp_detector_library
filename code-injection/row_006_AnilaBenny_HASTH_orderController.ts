import { Request, Response } from 'express';
import logger from '../../../../logger';


export default (dependencies: any) => {
  const {orderUseCase } = dependencies.useCase;

  const orderController = async (req: Request, res: Response) => {
    try {


      const executionFunction = await orderUseCase(dependencies);
      const response = await executionFunction.executionFunction(req.body);

  if (response.status) {
   
        res.json({ status: true , data: response.data });
      } else  {
        res.json({ status: false });
      }

    } catch (error) {
      logger.error('Error in order controller:', error);
      res.status(500).json({ message: 'Internal server error' });
    }
  };

  return orderController;
};
