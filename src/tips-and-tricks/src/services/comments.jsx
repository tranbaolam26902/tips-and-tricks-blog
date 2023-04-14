import axios from 'axios';

export async function sendComment(comment) {
	const { data } = await axios.post(
		`${process.env.REACT_APP_API_ENDPOINT}/comments?id=${
			comment.id
		}&name=${encodeURIComponent(
			comment.name,
		)}&description=${encodeURIComponent(comment.description)}`,
	);

	if (data.isSuccess) return data.result;
	else return null;
}

export async function getCommentsByQueries(queries) {
	const { data } = await axios.get(
		`${process.env.REACT_APP_API_ENDPOINT}/comments?${queries}`,
	);

	if (data.isSuccess) return data.result;
	else return null;
}

export async function deleteCommentById(id) {
	const { data } = await axios.delete(
		`${process.env.REACT_APP_API_ENDPOINT}/comments/${id}`,
	);

	return data;
}

export async function getCommentById(id = 0) {
	const { data } = await axios.get(
		`${process.env.REACT_APP_API_ENDPOINT}/comments/${id}`,
	);

	if (data.isSuccess) return data.result;
	else return null;
}

export async function toggleCommentApprovalStatus(id) {
	const { data } = await axios.post(
		`${process.env.REACT_APP_API_ENDPOINT}/comments/${id}`,
	);

	return data;
}
