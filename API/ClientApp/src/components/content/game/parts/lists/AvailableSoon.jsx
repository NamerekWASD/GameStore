import { useEffect, useState } from "react"
import { loadFilterData, loadGamesByFilters } from "../../../../../utils/ApiRequests"
import GameList from "../GameList";

export const AvailableSoon = () => {
    const [games, setGames] = useState([]);
    useEffect(() => {
        loadGamesByFilters({ orderBy: 1, dateFrom: new Date() }, 1).then(result => {
            result.games.length = Math.min(result.games.length, 5)
            setGames(result.games)
        })
    }, [])

    return (
        <>
            <div className='bg-gray-gradient-top w-100' style={{ height: '20px' }}></div>

            <div className="bg-dark p-1 py-5">
                <h1 className="text-center text-white">Скоро у продажі</h1>
                <div className="d-flex justify-content-center">
                    <GameList games={games} isMax isVertical noNeedPagination cardClassName={"bg-dark border border-white"} />
                </div>
            </div>

            <div className='bg-gray-gradient-bottom w-100' style={{ height: '20px' }}></div>
        </>
    )
}