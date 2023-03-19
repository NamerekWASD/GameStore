import { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { MANAGER } from "../../../utils/Constants";
import { verify } from "../../../utils/Navigation";
import ExtendedGameList from "./parts/ExtendedGameList";

const Manager = () => {
    const navigate = useNavigate();

    useEffect(() => {
        if (!verify(MANAGER)) {
            navigate('/')
        }
    }, [navigate])

    return (
        <main>
            <div>
                <ExtendedGameList />
            </div>
        </main>
    )
}
export default Manager;