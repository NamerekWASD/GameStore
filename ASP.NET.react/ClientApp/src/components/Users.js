import React, { useState, useEffect } from 'react';


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
        (users.length !== 0) ?
            <main>
            {
                    users.map((user) => <h3 key={`user-${user.id}`}>{user.name}</h3>)
            }
            </main>
            : <main><h3>Loading...</h3></main>
        )
}


export default Users;