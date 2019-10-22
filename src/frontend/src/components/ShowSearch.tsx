import React from 'react';
import AsyncSelect from 'react-select/async';
import ApiClient from '../api/ApiClient';
import history from "../utils/history";
import {ValueType, ActionMeta} from 'react-select/src/types';
import {useAuth0} from '../providers/Auth0Provider';



export default function ShowSearch() {
    const { getTokenSilently } = useAuth0();

    const loadOptions =  async (inputValue: string) => {
        if (!inputValue) {
            return [];
          }  
          const token = await getTokenSilently() as string;
          const response = await ApiClient.searchShows(token, inputValue);
          return response.data.map(x => {
              return {
                  label: x.title,
                  value: x.imdbId
              }
          });
    };

    const handleChange = async (value: ValueType<any>, action: ActionMeta) =>  {
        if(action.action === "select-option") {
            const imdbId = value.value;
            const token = await getTokenSilently() as string;
            const response = await ApiClient.startWatching(token, imdbId);

            history.push(`/show/${response.data.id}`);
        }
    }

    return (
        <AsyncSelect
            cacheOptions
            loadOptions={loadOptions}
            defaultOptions
            onChange={handleChange}
        />
    );
}