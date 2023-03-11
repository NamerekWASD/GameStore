import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { loadGames } from "../../../utils/ApiRequests";
import { navigateToDetails } from '../../../utils/Navigation';
import './game.css';

const GameList = () => {
    const [games, setGames] = useState([]);
    const navigate = useNavigate();

    useEffect(() => {
        if (games.length === 0) {
            loadGames(false).then(result => setGames(result));
        }
    }, [games]);

    function renderGames() {
        return (
            <>
                {
                    (games.length !== 0) ?
                        games.map(game => {
                            return (
                                <div key={game.id} className="col rounded-0" style={{ maxWidth: "250px" }}>
                                    <div className="card h-100">
                                        <img style={{ height: "250px" }} className="img-link" src={game.imageURL} alt={game.title} onClick={() => navigateToDetails(game, navigate)} />
                                        <div className="card-body">
                                            <h6 className="card-title text-center">{game.title} ({game.platform} {game.copyType})</h6>
                                        </div>
                                        <h5 className="card-text fw-bold mt-auto text-center">{game.price}<sup>$</sup></h5>
                                        <button className="btn btn-outline-dark rounded-0" onClick={() => navigateToDetails(game, navigate)}>Деталі</button>
                                    </div>
                                </div>
                            )
                        }) : <></>
                }
            </>
        )
    }

    return (
        <>
            <div className="row row-cols-1 row-cols-md-3 my-3 g-4 justify-content-center flex-wrap">
                {renderGames()}
            </div>
        </>
    )
}
export default GameList;