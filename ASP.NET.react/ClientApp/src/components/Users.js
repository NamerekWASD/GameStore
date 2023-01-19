import React, { useState, useEffect } from 'react';
import { Button } from '../../node_modules/reactstrap/types/index';


const Users = () => {

    const [users, setUsers] = useState([]);
    const userName = 'Namerek';

    useEffect(() => {
        fetchFromAPI();
    }, [userName]);

    function fetchFromAPI() {
        fetch(`user/${userName}`)
            .then((results) => {
                return results.json();
            })
            .then(data => {
                setUsers(data);
            })
    }


    return (
        (users != null) ?
            <main>
            {
                    users.map((user) => <h3 key={`user-${user.id}`}>{user.name}</h3>)
                    <button></button>
            }
            </main>
            : <main><h3>Loading...</h3></main>
        )
}


export default Users;