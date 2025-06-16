import React, { useState } from 'react';
import axios from 'axios';

function Order() {
  const [orderId, setOrderId] = useState('');
  const [order, setOrder] = useState(null);
  const [message, setMessage] = useState('');

  const fetchOrder = async (e) => {
    e.preventDefault();
    // Vulnerable: No auth, IDOR, XSS in message
    try {
      const res = await axios.get(`/api/product/order/${orderId}`);
      setOrder(res.data);
      setMessage('Order loaded!');
    } catch (err) {
      setMessage('Error: ' + err.message);
    }
  };

  return (
    <div>
      <h2>Order Lookup</h2>
      <form onSubmit={fetchOrder}>
        <input value={orderId} onChange={e => setOrderId(e.target.value)} placeholder="Order ID" />
        <button type="submit">Lookup</button>
      </form>
      <div dangerouslySetInnerHTML={{ __html: message }} />
      {order && (
        <div>
          <h3>Order #{order.id}</h3>
          <pre>{JSON.stringify(order, null, 2)}</pre>
        </div>
      )}
    </div>
  );
}

export default Order; 