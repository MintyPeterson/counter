// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'entry_list_entry_response.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

EntryListEntryResponse _$EntryListEntryResponseFromJson(
        Map<String, dynamic> json) =>
    EntryListEntryResponse(
      entryId: json['entryId'] as String,
      entryDate: DateTime.parse(json['entryDate'] as String),
      entry: json['entry'] as int,
      notes: json['notes'] as String?,
      isEstimate: json['isEstimate'] as bool,
    );
