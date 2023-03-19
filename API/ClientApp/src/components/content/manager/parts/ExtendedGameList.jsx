import { useEffect, useMemo, useState } from "react";
import { useNavigate } from "react-router-dom";
import { loadGames } from "../../../../utils/ApiRequests";
import { AppPaths } from "../../../../utils/AppPaths";
import { PORTRAIT } from "../../../../utils/Constants";
import LoadingCircle from "../../../../utils/LoadingCircle";
import FilterTable from "../../game/parts/FilterTable";
import Price from "../../game/parts/Price";

const ExtendedGameList = () => {
    const navigate = useNavigate();
    const [games, setGames] = useState([]);

    useEffect(() => {
        loadGames(true).then(result => setGames(result));
    }, [])


    const createGame = () => {
        navigate(AppPaths.createGame);
    }

    const renderGames = () => {
        const editGame = (game) => {
            navigate(AppPaths.editGame + '?' + new URLSearchParams([["gameId", game.id]]))
        }
    
        return (
            <>
                {

                    games ? games.map(game => {
                        return (
                            <tr key={game.id} className="pointer" onClick={() => editGame(game)}>
                                <td><img src={game.images.find(item => item.name === PORTRAIT).path} style={{ width: '150px' }} alt={game.title} /></td>
                                <td>{game.id}</td>
                                <td><h5>{game.title}</h5><span>{game.genres.join(', ')}</span></td>
                                <td>{game.copyType}</td>
                                <td className={game.copyCount <= 3 ? "text-danger" : ""}>{game.copyCount}</td>
                                <td><Price item={game} /></td>
                            </tr>
                        )
                    })
                    :
                    <LoadingCircle />
                }
            </>
        )
    }
    const memoGames = useMemo(renderGames, [games, navigate])
    return (
        <div>
            <div>
                <div>
                    <button className="btn btn-outline-success rounded-0 m-3"
                        onClick={createGame}>Створити продукт</button>
                </div>
            </div>
            <div>
                <h3>Знайдено {games ? games.length : 'жодної гри'} {games && games.length.toString().endsWith('1') ? 'гра' : 'гри'}</h3>
            </div>

            <div className="d-flex">
                {
                    games.length !== 0 ?
                        <table className=" responsive-table">
                            <thead className=" sticky-top" style={{ top: '55px' }}>
                                <tr>
                                    <th></th>
                                    <th>Id</th>
                                    <th>Назва (жанри)</th>
                                    <th>Тип копії</th>
                                    <th>Кількість копій</th>
                                    <th>Ціна</th>
                                </tr>
                            </thead>
                            <tbody>
                                {memoGames}
                            </tbody>
                        </table>
                        :
                        <h2>За вашим запитом не знайдено жодної гри!</h2>
                }
                <div>
                    <FilterTable setGames={setGames} />
                </div>
            </div>
        </div>
    )
}
export default ExtendedGameList;