import { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { uploadChanges } from "../../../utils/ApiRequests";
import { MANAGER } from "../../../utils/Constants";
import { verify } from "../../../utils/Navigation";
import GameForm from "./parts/GameForm";


const EditGame = () => {
    const navigate = useNavigate();
    useEffect(() => {
        if (!verify(MANAGER)) {
            navigate('/')
        }
    }, [navigate])

    const saveChanges = async (game) => {
        console.log(game);
        const response = await uploadChanges(game);
        console.log(await response.json());
    }

    return (
        <>
            {
                <GameForm saveChanges={saveChanges}/>
            }
        </>
    )
}
export default EditGame;