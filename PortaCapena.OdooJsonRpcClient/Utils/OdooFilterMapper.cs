using System;
using System.Linq;
using PortaCapena.OdooJsonRpcClient.Consts;
using PortaCapena.OdooJsonRpcClient.Extensions;
using PortaCapena.OdooJsonRpcClient.Models;
using PortaCapena.OdooJsonRpcClient.Request;

namespace PortaCapena.OdooJsonRpcClient.Utils
{
    public class OdooFilterMapper
    {
        public static OdooFilter ToOdooExpresion<T>(string propertyname, OdooOperator odooOperator, object value) where T : IOdooModel, new()
        {
            return new OdooFilter { OdooExtensions.GetOdooPropertyName<T>(propertyname), odooOperator.Description(), value };
        }

        public static OdooFilter ToOdooExpresion<T>(object left, OdooOperator odooOperator, object right)
            where T : IOdooModel, new()
        {
            return ToOdooExpresion<T>(left, odooOperator, right, 0);
        }

        private static OdooFilter ToOdooExpresion<T>(object left, OdooOperator odooOperator, object right, int depth) 
            where T : IOdooModel, new()
        {
            if (depth > 100) //should always be safe
            {
                throw new Exception("Invalid expression: OdooFilter depth exceeds 100");
            }
            
            if (left is string leftMember)
            {
                return ToOdooExpresion<T>(leftMember, odooOperator, right);
            }
    
            if (right is string rightMember)
            {
                return ToOdooExpresion<T>(rightMember, odooOperator, left);
            }

            if (!(left is object[] leftArray) || !(right is object[] rightArray))
            {
                throw new ArgumentException("Invalid expression: Unsupported operand types");
            }

            if (!IsValidOdooArray(leftArray) || !IsValidOdooArray(rightArray))
            {
                throw new ArgumentException("Invalid expression: Arrays must have exactly 3 non-null elements with operator at index 1");
            }
        
            var leftFilter = ToOdooExpresion<T>(leftArray[0], (OdooOperator)leftArray[1], leftArray[2], ++depth);
            var rightFilter = ToOdooExpresion<T>(rightArray[0], (OdooOperator)rightArray[1], rightArray[2], depth);
            
            return new OdooFilter 
            { 
                leftFilter,
                odooOperator.Description(),
                rightFilter
            };
        }

        private static bool IsValidOdooArray(object[] array)
        {
            return array != null 
                && array.Length == 3 
                && array[0] != null 
                && array[1] is OdooOperator
                && array[2] != null;
        }
    }
}