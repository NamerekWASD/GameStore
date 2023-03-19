import { useEffect, useRef, useState } from "react";
import { useNavigate } from "react-router-dom";
import { loadGamesBySearchQuery } from "../../../utils/ApiRequests";
import { navigateToDetails } from "../../../utils/Navigation";

const GameDropList = ({ searchQuery, refresh }) => {
    const navigate = useNavigate();
    const [games, setGames] = useState([]);
    const cardContainer = useRef(null);
    const gameDropList = useRef(null);
    useEffect(() => {
        if (searchQuery.length !== 0) {
            loadGamesBySearchQuery(searchQuery).then(result => setGames(result));
            gameDropList.current.style.height = cardContainer.current.offsetHeight + 'px';
        } else {
            gameDropList.current.style.height = "0px";
        }
    }, [searchQuery])

    useEffect(() => {
        if (searchQuery.length !== 0) {
            setTimeout(() => {
                gameDropList.current.style.height = cardContainer.current.offsetHeight + 'px';
            }, 20);
        }
    }, [games])

    function handleNavigate(game) {
        refresh();
        navigateToDetails(game, navigate);
    }

    return (
        <div ref={gameDropList} className="drop-list">
            <div ref={cardContainer}>
                {
                    games.length !== 0 ?
                        games.map((game) => {
                            return (
                                <div key={game.id} className="card rounded-0 " style={{ cursor: 'pointer', width: '500px' }}
                                    onClick={() => handleNavigate(game)} >
                                    <div className="no-gutters d-flex flex-row align-items-center p-2 gap-5">
                                        <div>
                                            <img src={game.imageURL} alt={game.title} style={{ width: '100px' }} />
                                        </div>
                                        <div className="flex-fill">
                                            <h6 className="card-title m-0">{game.title}</h6>
                                        </div>
                                        <div>
                                            <span className="fs-3">{game.price}<sup>$</sup></span>
                                        </div>
                                    </div>
                                </div>
                            )
                        })
                        :
                        <div className="card rounded-0" style={{ width: '500px' }}>
                            <h5 className="m-0">За вашим Запитом не знайдено жодної гри.</h5>
                        </div>
                }
            </div>
        </div >
    )
}

export default GameDropList;