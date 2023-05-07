import React from "react";
import { useNavigate } from "react-router-dom";
import { AppPaths } from "../../../utils/AppPaths";
const Order = ({ data }) => {
    const navigate = useNavigate();
    const options = { year: 'numeric', month: 'long', day: 'numeric' };
    const totalPrice = data.copies.reduce((sum, element) => sum + element.price, 0);
    const created = new Date(data.created).toLocaleDateString(undefined, options);

    const navigateToOrderDetails = () =>{
        navigate(AppPaths.orderDetails + '?' + new URLSearchParams([['orderNumber', data.orderNumber]]).toString());
    }

    return (
            <tr className="pointer" onClick={navigateToOrderDetails}>
                <td className="text-smaller">{data.orderNumber}</td>
                <td>{parseFloat(totalPrice).toFixed(2)}$</td>
                <td>{data.copies.length}</td>
                <td>{created}</td>
            </tr>
    )
}
export default Order;