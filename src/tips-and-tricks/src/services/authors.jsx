import axios from 'axios';

import { API_URL } from '../utils/constants';

export async function getAuthorBySlug(slug) {
	const { data } = await axios.get(`${API_URL}/authors/${slug}`);

	if (data.isSuccess) return data.result;
	else return null;
}

export async function getAuthors() {
	const { data } = await axios.get(
		`${API_URL}/authors?PageSize=1000&PageNumber=1`,
	);

	if (data.isSuccess) return data.result;
	else return null;
}
