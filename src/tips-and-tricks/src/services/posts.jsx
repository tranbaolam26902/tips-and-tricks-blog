import axios from 'axios';

import { API_URL } from '../utils/constants';

export async function getPostsByQueries(queries) {
	const { data } = await axios.get(`${API_URL}/posts?${queries}`);

	if (data.isSuccess) return data.result;
	else return null;
}

export async function getPostBySlug(slug) {
	const { data } = await axios.get(`${API_URL}/posts/byslug/${slug}`);

	if (data.isSuccess) return data.result;
	else return null;
}

export async function getPostComments(id) {
	const { data } = await axios.get(`${API_URL}/posts/${id}/comments`);

	if (data.isSuccess) return data.result;
	else return null;
}
