import { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { MANAGER } from "../../../utils/Constants";
import { verify } from "../../../utils/Navigation";
import GameForm from "./parts/GameForm";


const CreateGame = () => {
    const navigate = useNavigate();

    useEffect(() => {
        if (!verify(MANAGER)) {
            navigate('/')
        }
    }, [navigate])

    const saveChanges = (newData) => {
        console.log(newData);
    }

    return (
        <>
            <GameForm saveChanges={saveChanges} isCreate />
        </>
    )
}
export default CreateGame;