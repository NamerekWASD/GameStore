import { useEffect, useState } from "react"
import { loadFilterData, loadGamesByFilters } from "../../../../../utils/ApiRequests"
import GameList from "../GameList";

export const Novelties = () => {
    const [games, setGames] = useState([]);
    useEffect(() => {
        loadGamesByFilters({ orderBy: 1, dateTo: new Date() }, 1).then(result => {
            result.games.length = Math.min(result.games.length, 3)
            setGames(result.games)
        })
    }, [])

    return (
        <div className="bg-dark p-1">
            <h3 className="text-center text-white">Новинки</h3>
            <div className="text-center">
                <GameList games={games} isColumn isMax isVertical noNeedPagination cardClassName={"bg-dark border border-white"} />
            </div>
        </div>
    )
}