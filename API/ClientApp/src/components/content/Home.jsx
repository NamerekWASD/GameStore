import React, { useEffect } from 'react';
import GameList from './game/GameList';
import './Home.css'

const Home = (props) => {

  return (
    <div id='home' className='container'>
        <GameList />
    </div>
  );
}
export default Home;