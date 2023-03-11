import React from "react";
const Order = ({ data }) => {
    const totalPrice = data.games.reduce((sum, element) => sum + element.price);
    return (
            <tr>
                <td>{data.id}</td>
                <td>{data.totalPrice}</td>
            </tr>
    )
}
export default Order;