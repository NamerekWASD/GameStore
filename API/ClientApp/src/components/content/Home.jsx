import React, { useEffect, useState } from 'react';
import Carousel from './game/parts/Carousel';
import { loadGames } from '../../utils/ApiRequests';
import Loading from '../../utils/Loading';
import GameList from './game/parts/GameList';

const Home = (props) => {
  const [games, setGames] = useState([]);
  useEffect(() => {
    loadGames(false).then(result => setGames(result));
  }, []);
  const copies = [...games].filter(item => item.isHotOffer).concat([...games].filter(item => item.isHotOffer))
  return (
    games.length !== 0 ?
      <div id='home' className='container'>
        <div className='bg-dark'>
          <div>
            <Carousel games={copies} />
          </div>
        </div>
        <GameList games={games} />
      </div>
      :
      <Loading />
  );
}
export default Home;