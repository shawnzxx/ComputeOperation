using System;
using System.Collections.Generic;
using System.Text;

namespace Compute.Domain.Models
{
    public class Operation
    {

        private OperationTypeEnum _operationType;

        public int Id { get; set; }
        public double X { get; private set; }
        public double Y { get; private set; }
        public double Result { get; private set; }
        public OperationTypeEnum OperationType { get; private set; }

        //Constructor for the Operation
        public Operation(double x, double y, OperationTypeEnum operationType)
        {
            X = x;
            Y = y;
            OperationType = operationType;
            CalculateResult();
        }

        private void CalculateResult()
        {
            switch (_operationType)
            {
                case OperationTypeEnum.Add:
                    Result = X + Y;
                    break;
                case OperationTypeEnum.Sub:
                    Result = X - Y;
                    break;
                case OperationTypeEnum.Mul:
                    Result = X * Y;
                    break;
                case OperationTypeEnum.Div:
                    Result = X / Y;
                    break;
            }
        }

        //Type of the operation
        public enum OperationTypeEnum
        {
            Add = 1,
            Sub = 2,
            Mul = 3,
            Div = 4
        }
    }
}
