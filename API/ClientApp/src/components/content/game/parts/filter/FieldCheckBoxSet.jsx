import React from "react";
import $ from 'jquery';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faArrowDown } from "@fortawesome/free-solid-svg-icons";

const FieldCheckBoxSet = ({ fieldName, array, propertyName, processInput }) => {

    const handleCollapse = (e) => {
        const element = $(`#${propertyName}-collapse`);
        element.animate({ height: 'toggle' }, 200);
        $(`#${propertyName}-icon-collapse`).toggleClass("icon-down");
    }

    return (
        <div className="mb-1 bg-gray p-3">
            <h5 className="text-center pointer position-relative p-1 border-bottom border-1 border-dark" onClick={handleCollapse}>{fieldName}
                <FontAwesomeIcon id={propertyName + "-icon-collapse"} icon={faArrowDown} className='position-absolute end-0 me-3 icon-normal icon-down' />
            </h5>
            <div id={propertyName + "-collapse"} className="dis-grid grid-2 position-relative">
                {
                    array.map((value) => {
                        return (
                            <div key={value.id} className="form-group checkbox-unique">
                                <input id={propertyName + "|" + value.id} type="checkbox"
                                    onChange={(e) => processInput(value.id, e.target.checked, propertyName)} />
                                <label htmlFor={propertyName + "|" + value.id}>{value.name}</label>
                            </div>
                        )
                    })
                }
            </div>
        </div>

    )
}
export default FieldCheckBoxSet;