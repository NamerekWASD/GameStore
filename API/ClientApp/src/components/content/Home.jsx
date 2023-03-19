import React, { useEffect, useState } from 'react';
import Carousel from './game/parts/Carousel';
import { loadGames } from '../../utils/ApiRequests';
import Loading from '../../utils/Loading';
import GameList from './game/parts/GameList';
import './Home.css'

const Home = (props) => {
  const [games, setGames] = useState([]);
  useEffect(() => {
    if (games.length === 0) {
      loadGames(false).then(result => setGames(result));
    }
  }, [games]);
  const copies = [...games].filter(item => item.isHotOffer).concat([...games].filter(item => item.isHotOffer))
  return (
    games.length !== 0 ?
      <div id='home' className='container'>
        <div className=' bg-dark'>
          <Carousel games={copies}/>
        </div>
        <GameList games={games} />
      </div>
      :
      <Loading />
  );
}
export default Home;