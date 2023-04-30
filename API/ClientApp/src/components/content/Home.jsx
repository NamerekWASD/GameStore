import React, { useEffect, useState } from 'react';
import Carousel from './game/parts/Carousel';
import { GetGames } from '../../utils/ApiRequests';
import Loading from '../../utils/Loading';
import GameList from './game/parts/GameList';
import { Novelties } from './game/parts/lists/Novelties';
import { isMobile } from 'react-device-detect';
import { AvailableSoon } from './game/parts/lists/AvailableSoon';

export const findUnique = (games, value) => {
    if (games.some(game => game.id === value.id)) {
        return false;
    }
    return true;
}

const Home = (props) => {
    const [games, setGames] = useState([]);
    const [isMax, setIsMax] = useState(false);
    const [page, setPage] = useState(1);

    useEffect(() => {
        GetGames(page).then(result => {
            if (result.page !== 1) {
                setGames(prevState => [...prevState, ...result.games.filter(game => findUnique(games, game))]);
            } else {
                setGames(result.games)
            }
            setIsMax(result.isMax);
        });
        if (page < 3 && !isMax) {
            setPage(prevState => prevState + 1)
        }
    }, [page]);


    const copies = [...games].filter(item => item.isHotOffer).concat([...games].filter(item => item.isHotOffer))
    return (
        games.length !== 0 ?
            <div id='home'>
                <div className='bg-black'>
                    <div>
                        <Carousel games={copies} />
                    </div>
                </div>
                <div className='bg-gray-gradient-bottom w-100' style={{ height: '50px' }}></div>
                <div className='d-flex flex-row mt-3 container'>
                    <div className='me-2 flex-fill'>
                        <GameList games={games} isMax noNeedPagination />
                    </div>
                    <div className='vertical-card-holder'>
                        {
                            isMobile ? "" :
                                <Novelties />
                        }
                    </div>
                </div>
                <div>
                    <AvailableSoon />
                </div>
            </div>
            :
            <Loading />
    );
}
export default Home;