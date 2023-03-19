import React from "react";
import { useNavigate } from "react-router-dom";
import { navigateToDetails } from '../../../../utils/Navigation';
import './../game.css';
import Price from "./Price";

const GameList = ({games}) => {
    const navigate = useNavigate();

    function renderGames() {
        return (
            <>
                {
                    (games.length !== 0) ?
                        games.map(game => {
                            return (
                                <div key={game.id} className="card rounded-0 w-100" style={{ cursor: 'pointer', height: '180px' }}
                                    onClick={() => navigateToDetails(game, navigate)} >
                                    <div className="no-gutters d-flex flex-row align-items-center gap-5">
                                        <div style={{minWidth: "120px", width: "120px", maxWidth: "120px", height: '150px'}} className="overflow-hidden m-3">
                                            <img src={game.image ? game.image.path : game.images.find(item => item.name === 'portrait').path} alt={game.title} className="width-inherit"/>
                                        </div>
                                        <div className="flex-fill">
                                            <h6 className="card-title">{game.title}</h6>
                                            <h6 className="text-muted">{game.genres.join(' ')}</h6>
                                        </div>
                                        <div className="p-3">
                                            <Price item={game}/>
                                        </div>
                                    </div>
                                </div>
                            )
                        }) : <></>
                }
            </>
        )
    }

    return (
        <main>
            <div className="g-4 justify-content-center flex-wrap">
                {renderGames()}
            </div>
        </main>
    )
}
export default GameList;