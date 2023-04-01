import { useScrollPosition } from "@n8tb1t/use-scroll-position";
import { useEffect, useRef, useState } from "react";
import { useNavigate } from "react-router-dom";
import { loadGamesByFilters } from "../../../../utils/ApiRequests";
import { AppPaths } from "../../../../utils/AppPaths";
import { PORTRAIT } from "../../../../utils/Constants";
import LoadingCircle from "../../../../utils/LoadingCircle";
import { processHeader } from "../../game/GameCatalog";
import FilterTable from "../../game/parts/filter/FilterTable";
import Price from "../../game/parts/Price";

const ExtendedGameList = () => {
    const navigate = useNavigate();
    const [searchFilters, setSearchFilters] = useState();

    const [games, setGames] = useState([]);
    const [isMax, setIsMax] = useState(false);
    const [page, setPage] = useState(1);
    const [count, setCount] = useState(0);
    const listContainer = useRef(null)
    useEffect(() => {
        if (page === 0) {
            setPage(1);
            return;
        }
        loadGamesByFilters(searchFilters, page).then(result => {
            setGames(prevState => [...prevState, ...result.games])
            setIsMax(result.isMax);
            setCount(result.totalCount);
        });
    }, [page, searchFilters])

    const createGame = () => {
        navigate(AppPaths.createGame);
    }
    const editGame = (game) => {
        navigate(AppPaths.editGame + '?' + new URLSearchParams([["gameId", game.id]]))
    }
    useScrollPosition(({ prevPos, currPos }) => {
        const rect = listContainer.current.getBoundingClientRect();
        const bottomPosition = rect.height + rect.y + currPos.y - window.innerHeight;
        if (bottomPosition < 0 && !isMax) {
            console.log(isMax);
            setPage(prevState => prevState + 1);
        }

    }, [isMax], false, false, 300)
    return (
        <div className="container">
            <div className="p-3">
                <button className="btn btn-outline-success rounded-0 w-100" style={{ height: '4rem' }}
                    onClick={createGame}>Створити продукт</button>
            </div>
            <div>
                <h3>Знайдено {processHeader(count)}</h3>
            </div>

            <div className="d-flex">
                <div className="flex-fill me-2">
                    {
                        games && games.length !== 0 ?
                            <table className="responsive-table">
                                <thead className="sticky-top" style={{ top: '55px' }}>
                                    <tr>
                                        <th></th>
                                        <th>Id</th>
                                        <th>Назва (жанри)</th>
                                        <th>Тип копії</th>
                                        <th>Кількість копій</th>
                                        <th>Ціна</th>
                                    </tr>
                                </thead>
                                <tbody ref={listContainer}>
                                    {

                                        games.map(game => {
                                            return (
                                                <tr key={game.id} className="pointer" onClick={() => editGame(game)} style={{ maxWidth: '150px' }}>
                                                    <td><img src={game.images.find(item => item.type.name === PORTRAIT).path} style={{ width: '150px' }} alt={game.title} /></td>
                                                    <td>{game.id}</td>
                                                    <td><h5>{game.title}</h5><span>{game.genres.join(', ')}</span></td>
                                                    <td>{game.copyType}</td>
                                                    <td className={game.copyCount <= 3 ? "text-danger" : ""}>{game.copyCount}</td>
                                                    <td><Price item={game} /></td>
                                                </tr>
                                            )
                                        })
                                    }
                                </tbody>
                            </table>
                            :
                            <h2>За вашим запитом не знайдено жодної гри!</h2>
                    }
                    {
                        !isMax ?
                            <LoadingCircle />
                            : ''
                    }
                </div>
                <div>
                    <FilterTable setGames={setGames}
                        setSearchFilters={setSearchFilters}
                        setPage={setPage}
                        searchFilters={searchFilters}
                    />
                </div>
            </div>
        </div>
    )
}
export default ExtendedGameList;