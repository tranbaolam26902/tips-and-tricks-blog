import axios from 'axios';

import { API_URL } from '../utils/constants';

export async function getAuthorBySlug(slug) {
	const response = await axios.get(`${API_URL}/api/authors/${slug}`);
	const data = response.data;

	if (data.isSuccess) return data.result;
	else return null;
}
